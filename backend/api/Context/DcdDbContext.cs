using api.Authorization;
using api.Models;
using api.Services;
using api.Services.EconomicsServices;
using api.Services.GenerateCostProfiles;
using api.StartupConfiguration;

using Microsoft.EntityFrameworkCore;


namespace api.Context;

public class DcdDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider = null!;
    private readonly CurrentUser? _currentUser;

    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public DcdDbContext(DbContextOptions<DcdDbContext> options,
        CurrentUser? currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DcdDbContext(DbContextOptions<DcdDbContext> options,
                        IServiceProvider serviceProvider,
                        CurrentUser currentUser) : base(options)
    {
        _serviceProvider = serviceProvider;
        _currentUser = currentUser;
    }

    // TODO: This is not pretty, need to move this logic out of the context
    public async Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            await DetectChangesAndCalculateEntities(caseId);

            bool saveFailed;
            int result = 0;

            do
            {
                saveFailed = false;
                try
                {
                    result = await SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Iterate over all the entries in the exception
                    foreach (var entry in ex.Entries)
                    {
                        var databaseEntry = entry.GetDatabaseValues();

                        if (databaseEntry == null)
                        {
                            throw new Exception("The entity being updated has been deleted.");
                        }
                        else
                        {
                            var databaseValues = databaseEntry.ToObject();

                            // TODO: Decide how to handle the conflict
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }
            } while (saveFailed);

            return result;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task DetectChangesAndCalculateEntities(Guid caseId)
    {
        var (wells, drillingScheduleIds) = CalculateExplorationAndWellProjectCost();
        var rerunStudyCost = CalculateStudyCost();
        var rerunCessationCostProfile = CalculateCessationCostProfile();
        var rerunFuelFlaringAndLosses = CalculateFuelFlaringAndLosses();
        var rerunGAndGAdminCost = CalculateGAndGAdminCost();
        var rerunImportedElectricity = CalculateImportedElectricity();
        var rerunNetSalesGas = CalculateNetSalesGas();
        var rerunOpex = CalculateOpex();
        var rerunCo2Emissions = CalculateCo2Emissions();
        var rerunTotalIncome = CalculateTotalIncome();
        var rerunTotalCost = CalculateTotalCost();
        var rerunCalculateNPV = CalculateNPV();
        var rerunCalculateBreakEven = CalculateBreakEvenOilPrice();

        await SaveChangesAsync(); // TODO: This is a hack to find the updated values in the calculate services. Need to find a better way to do this.
        if (wells.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await _serviceProvider.GetRequiredService<IWellCostProfileService>().UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            await _serviceProvider.GetRequiredService<IWellCostProfileService>().UpdateCostProfilesForWells(wells);
        }
        if (rerunStudyCost)
        {
            await _serviceProvider.GetRequiredService<IStudyCostProfileService>().Generate(caseId);
        }

        if (rerunCessationCostProfile)
        {
            await _serviceProvider.GetRequiredService<ICessationCostProfileService>().Generate(caseId);
        }

        if (rerunFuelFlaringAndLosses)
        {
            await _serviceProvider.GetRequiredService<IFuelFlaringLossesProfileService>().Generate(caseId);
        }

        if (rerunGAndGAdminCost)
        {
            await _serviceProvider.GetRequiredService<IGenerateGAndGAdminCostProfile>().Generate(caseId);
        }

        if (rerunImportedElectricity)
        {
            await _serviceProvider.GetRequiredService<IImportedElectricityProfileService>().Generate(caseId);
        }

        if (rerunNetSalesGas)
        {
            await _serviceProvider.GetRequiredService<INetSaleGasProfileService>().Generate(caseId);
        }

        if (rerunOpex)
        {
            await _serviceProvider.GetRequiredService<IOpexCostProfileService>().Generate(caseId);
        }

        if (rerunCo2Emissions)
        {
            await _serviceProvider.GetRequiredService<ICo2EmissionsProfileService>().Generate(caseId);
        }

        if (rerunTotalIncome)
        {
            var calculateIncomeHelper = _serviceProvider.GetRequiredService<ICalculateTotalIncomeService>();
            await calculateIncomeHelper.CalculateTotalIncome(caseId);
        }

        if (rerunTotalCost)
        {
            var calculateCostHelper = _serviceProvider.GetRequiredService<ICalculateTotalCostService>();
            await calculateCostHelper.CalculateTotalCost(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateNPV)
        {
            var calculateNPVHelper = _serviceProvider.GetRequiredService<ICalculateNPVService>();
            await calculateNPVHelper.CalculateNPV(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateBreakEven)
        {
            var calculateBreakEvenHelper = _serviceProvider.GetRequiredService<ICalculateBreakEvenOilPriceService>();
            await calculateBreakEvenHelper.CalculateBreakEvenOilPrice(caseId);
        }
    }

    private (List<Well> wells, List<Guid> drillingScheduleIds) CalculateExplorationAndWellProjectCost()
    {
        var modifiedWellsWithCostChange = ChangeTracker.Entries<Well>()
            .Where(e => (e.State == EntityState.Modified)
                        && (e.Property(nameof(Well.WellCost)).IsModified || e.Property(nameof(Well.WellCategory)).IsModified));

        var modifiedWellIds = modifiedWellsWithCostChange.Select(e => e.Entity).ToList();

        var modifiedDrillingSchedules = ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => (e.State == EntityState.Modified)
                && (e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified
                || e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified));

        var addedDrillingSchedules = ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Added);

        var modifiedDrillingScheduleIds = modifiedDrillingSchedules.Select(e => e.Entity.Id)
                                            .Union(addedDrillingSchedules.Select(e => e.Entity.Id)).ToList();

        return (modifiedWellIds, modifiedDrillingScheduleIds);
    }

    private bool CalculateCo2Emissions()
    {
        var caseItemChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Topside.FuelConsumption)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.OilCapacity)).IsModified ||
                       e.Property(nameof(Topside.GasCapacity)).IsModified ||
                       e.Property(nameof(Topside.WaterInjectionCapacity)).IsModified));

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var wellChanges = ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return caseItemChanges
        || topsideChanges
        || productionProfileOilChanges
        || productionProfileOilAdded
        || additionalProductionProfileOilChanges
        || additionalProductionProfileOilAdded
        || productionProfileGasChanges
        || productionProfileGasAdded
        || additionalProductionProfileGasChanges
        || additionalProductionProfileGasAdded
        || productionProfileWaterInjectionChanges
        || productionProfileWaterInjectionAdded
        || wellChanges
        || drillingScheduleChanges
        || drillingScheduleAdded;
    }

    private bool CalculateOpex()
    {
        var historicCostChanges = ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.HistoricCostCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.HistoricCostCostProfile.InternalData)).IsModified);

        var historicCostAdded = ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var onshoreOpexChanges = ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.OnshoreRelatedOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.OnshoreRelatedOPEXCostProfile.InternalData)).IsModified);

        var onshoreOpexAdded = ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var additionalOpexChanges = ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.AdditionalOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.AdditionalOPEXCostProfile.InternalData)).IsModified);

        var additionalOpexAdded = ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var developmentOperationalWellCostsChanges = ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var wellsChanges = ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        var topsideOpexChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Topside.FacilityOpex)).IsModified);

        return historicCostChanges
            || historicCostAdded
            || onshoreOpexChanges
            || onshoreOpexAdded
            || additionalOpexChanges
            || additionalOpexAdded
            || developmentOperationalWellCostsChanges
            || productionProfileOilChanges
            || additionalProductionProfileOilChanges
            || productionProfileOilAdded
            || additionalProductionProfileOilAdded
            || productionProfileGasChanges
            || productionProfileGasAdded
            || additionalProductionProfileGasChanges
            || additionalProductionProfileGasAdded
            || wellsChanges
            || drillingScheduleChanges
            || drillingScheduleAdded
            || topsideOpexChanges;
    }

    private bool CalculateNetSalesGas()
    {
        var projectChanges = ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var caseItemChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Topside.FuelConsumption)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.OilCapacity)).IsModified ||
                       e.Property(nameof(Topside.GasCapacity)).IsModified ||
                       e.Property(nameof(Topside.WaterInjectionCapacity)).IsModified));

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var drainageStrategyChanges = ChangeTracker.Entries<DrainageStrategy>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrainageStrategy.GasSolution)).IsModified);

        return projectChanges
            || caseItemChanges
            || topsideChanges
            || productionProfileOilChanges
            || productionProfileOilAdded
            || additionalProductionProfileOilChanges
            || additionalProductionProfileOilAdded
            || productionProfileGasChanges
            || productionProfileGasAdded
            || additionalProductionProfileGasChanges
            || additionalProductionProfileGasAdded
            || productionProfileWaterInjectionChanges
            || productionProfileWaterInjectionAdded
            || drainageStrategyChanges;
    }

    private bool CalculateImportedElectricity()
    {
        var facilitiesAvailabilityChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Topside.CO2ShareOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxOilProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxGasProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2ShareWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.CO2OnMaxWaterInjectionProfile)).IsModified ||
                       e.Property(nameof(Topside.OilCapacity)).IsModified ||
                       e.Property(nameof(Topside.GasCapacity)).IsModified ||
                       e.Property(nameof(Topside.WaterInjectionCapacity)).IsModified));

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        return facilitiesAvailabilityChanges
            || topsideChanges
            || productionProfileOilChanges
            || productionProfileOilAdded
            || additionalProductionProfileOilChanges
            || additionalProductionProfileOilAdded
            || productionProfileGasChanges
            || productionProfileGasAdded
            || additionalProductionProfileGasChanges
            || additionalProductionProfileGasAdded
            || productionProfileWaterInjectionChanges
            || productionProfileWaterInjectionAdded;
    }

    private bool CalculateGAndGAdminCost()
    {
        var projectChanges = ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Project.Country)).IsModified);

        var caseChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                (e.Property(nameof(Case.DG4Date)).IsModified ||
                e.Property(nameof(Case.DG1Date)).IsModified));

        var wellsChanges = ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified
                || e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified);

        var drillingScheduleAdded = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return projectChanges || caseChanges || wellsChanges || drillingScheduleChanges || drillingScheduleAdded;
    }

    private bool CalculateFuelFlaringAndLosses()
    {
        var caseChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var projectChanges = ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var topsideChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Topside.FuelConsumption)).IsModified ||
                    e.Property(nameof(Topside.CO2ShareOilProfile)).IsModified ||
                    e.Property(nameof(Topside.CO2OnMaxOilProfile)).IsModified ||
                    e.Property(nameof(Topside.CO2ShareGasProfile)).IsModified ||
                    e.Property(nameof(Topside.CO2OnMaxGasProfile)).IsModified ||
                    e.Property(nameof(Topside.CO2ShareWaterInjectionProfile)).IsModified ||
                    e.Property(nameof(Topside.CO2OnMaxWaterInjectionProfile)).IsModified ||
                    e.Property(nameof(Topside.OilCapacity)).IsModified ||
                    e.Property(nameof(Topside.GasCapacity)).IsModified ||
                    e.Property(nameof(Topside.WaterInjectionCapacity)).IsModified
                ));

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified
                ));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified
                ));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified
                ));

        var productionProfileWaterInjectionAdded = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        return caseChanges
        || projectChanges
        || topsideChanges
        || productionProfileOilChanges
        || productionProfileOilAdded
        || additionalProductionProfileOilChanges
        || additionalProductionProfileOilAdded
        || productionProfileGasChanges
        || productionProfileGasAdded
        || additionalProductionProfileGasChanges
        || additionalProductionProfileGasAdded
        || productionProfileWaterInjectionChanges
        || productionProfileWaterInjectionAdded;
    }

    private bool CalculateCessationCostProfile()
    {
        var caseCessationWellsCostOverrideChanges = ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.CessationWellsCostOverride.InternalData)).IsModified
                || e.Property(nameof(Models.CessationWellsCostOverride.Override)).IsModified
            ));

        var cessationWellsCostAdded = ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var caseCessationOffshoreFacilitiesCostOverrideChanges = ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.CessationOffshoreFacilitiesCostOverride.InternalData)).IsModified
                || e.Property(nameof(Models.CessationOffshoreFacilitiesCostOverride.Override)).IsModified
            ));

        var cessationOffshoreFacilitiesCostAdded = ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified
                || e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified
            ));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
    .Any(e => e.State == EntityState.Modified &&
              (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
               e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = ChangeTracker.Entries<Surf>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Surf.CessationCost)).IsModified
            ));

        var developmentOperationalWellCostsChanges = ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.DevelopmentOperationalWellCosts.PluggingAndAbandonment)).IsModified
            ));
        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified
            ));

        var drillingScheduleAdded = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return caseCessationWellsCostOverrideChanges
            || cessationWellsCostAdded
            || caseCessationOffshoreFacilitiesCostOverrideChanges
            || cessationOffshoreFacilitiesCostAdded
            || productionProfileOilChanges
            || productionProfileOilAdded
            || additionalProductionProfileOilChanges
            || additionalProductionProfileOilAdded
            || productionProfileGasChanges
            || productionProfileGasAdded
            || additionalProductionProfileGasChanges
            || additionalProductionProfileGasAdded
            || surfChanges
            || developmentOperationalWellCostsChanges
            || drillingScheduleChanges
            || drillingScheduleAdded;
    }

    private bool CalculateStudyCost()
    {
        var caseChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Case.CapexFactorFeasibilityStudies)).IsModified
                || e.Property(nameof(Case.CapexFactorFEEDStudies)).IsModified
                || e.Property(nameof(Case.DG0Date)).IsModified
                || e.Property(nameof(Case.DG1Date)).IsModified
                || e.Property(nameof(Case.DG2Date)).IsModified
                || e.Property(nameof(Case.DG3Date)).IsModified
                || e.Property(nameof(Case.DG4Date)).IsModified
            ));

        var substructureChanges = ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.SubstructureCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.SubstructureCostProfileOverride.InternalData)).IsModified
            ));

        var substructureCostProfileAdded = ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.SurfCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.SurfCostProfileOverride.InternalData)).IsModified
            ));

        var surfCostProfileAdded = ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var topsideChanges = ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.TopsideCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.TopsideCostProfileOverride.InternalData)).IsModified
            ));

        var topsideCostProfileAdded = ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var transportChanges = ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.TransportCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.TransportCostProfileOverride.InternalData)).IsModified
            ));

        var transportCostProfileAdded = ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerChanges = ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.OilProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectOilProducerAdded = ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerOverrideChanges = ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.OilProducerCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.OilProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectOilProducerOverrideAdded = ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerChanges = ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasProducerAdded = ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerOverrideChanges = ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasProducerCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.GasProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasProducerOverrideAdded = ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorChanges = ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.WaterInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectWaterInjectorAdded = ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorOverrideChanges = ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.WaterInjectorCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.WaterInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectWaterInjectorOverrideAdded = ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorChanges = ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasInjectorAdded = ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorOverrideChanges = ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasInjectorCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.GasInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasInjectorOverrideAdded = ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        return caseChanges
            || substructureChanges
            || substructureCostProfileAdded
            || surfChanges
            || surfCostProfileAdded
            || topsideChanges
            || topsideCostProfileAdded
            || transportChanges
            || transportCostProfileAdded
            || wellProjectOilProducerChanges
            || wellProjectOilProducerAdded
            || wellProjectOilProducerOverrideChanges
            || wellProjectOilProducerOverrideAdded
            || wellProjectGasProducerChanges
            || wellProjectGasProducerAdded
            || wellProjectGasProducerOverrideChanges
            || wellProjectGasProducerOverrideAdded
            || wellProjectWaterInjectorChanges
            || wellProjectWaterInjectorAdded
            || wellProjectWaterInjectorOverrideChanges
            || wellProjectWaterInjectorOverrideAdded
            || wellProjectGasInjectorChanges
            || wellProjectGasInjectorAdded
            || wellProjectGasInjectorOverrideChanges
            || wellProjectGasInjectorOverrideAdded;
    }

    private bool CalculateTotalIncome()
    {
        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified
                ));

        var productionProfileOilAdded = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                        (e.Property(nameof(Models.AdditionalProductionProfileOil.InternalData)).IsModified ||
                        e.Property(nameof(Models.AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified
                ));

        var productionProfileGasAdded = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        return productionProfileGasChanges
            || productionProfileGasAdded
            || additionalProductionProfileGasChanges
            || additionalProductionProfileGasAdded
            || productionProfileOilChanges
            || productionProfileOilAdded
            || additionalProductionProfileOilChanges
            || additionalProductionProfileOilAdded;
    }

    private bool CalculateTotalCost()
    {

        var totalFeasibilityAdded = ChangeTracker.Entries<TotalFeasibilityAndConceptStudies>()
            .Any(e => e.State == EntityState.Added);

        var totalFeasibilityOverrideChanges = ChangeTracker.Entries<TotalFeasibilityAndConceptStudiesOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.TotalFeasibilityAndConceptStudiesOverride.Override)).IsModified
                || e.Property(nameof(Models.TotalFeasibilityAndConceptStudiesOverride.InternalData)).IsModified
            ));


        var totalFEEDChanges = ChangeTracker.Entries<TotalFEEDStudies>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalFEEDOverrideChanges = ChangeTracker.Entries<TotalFEEDStudiesOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalOtherStudiesChanges = ChangeTracker.Entries<TotalOtherStudiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var historicCostChanges = ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionChanges = ChangeTracker.Entries<WellInterventionCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionOverrideChanges = ChangeTracker.Entries<WellInterventionCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsChanges = ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsOverrideChanges = ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshoreRelatedOPEXChanges = ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var additionalOPEXChanges = ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsChanges = ChangeTracker.Entries<CessationWellsCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsOverrideChanges = ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesChanges = ChangeTracker.Entries<CessationOffshoreFacilitiesCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesOverrideChanges = ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOnshoreFacilitiesChanges = ChangeTracker.Entries<CessationOnshoreFacilitiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfChanges = ChangeTracker.Entries<SurfCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfOverrideChanges = ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureChanges = ChangeTracker.Entries<SubstructureCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureOverrideChanges = ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideChanges = ChangeTracker.Entries<TopsideCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideOverrideChanges = ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportChanges = ChangeTracker.Entries<TransportCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportOverrideChanges = ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerChanges = ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerOverrideChanges = ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerChanges = ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerOverrideChanges = ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorChanges = ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorOverrideChanges = ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorChanges = ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorOverrideChanges = ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminChanges = ChangeTracker.Entries<GAndGAdminCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminOverrideChanges = ChangeTracker.Entries<GAndGAdminCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var seismicChanges = ChangeTracker.Entries<SeismicAcquisitionAndProcessing>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var countryOfficeChanges = ChangeTracker.Entries<CountryOfficeCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var explorationWellChanges = ChangeTracker.Entries<ExplorationWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var appraisalWellChanges = ChangeTracker.Entries<AppraisalWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var sidetrackChanges = ChangeTracker.Entries<SidetrackCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        return
            totalFeasibilityAdded
            || totalFeasibilityOverrideChanges
            || totalFEEDChanges
            || totalFEEDOverrideChanges
            || totalOtherStudiesChanges
            || historicCostChanges
            || wellInterventionChanges
            || wellInterventionOverrideChanges
            || offshoreFacilitiesOperationsChanges
            || offshoreFacilitiesOperationsOverrideChanges
            || onshoreRelatedOPEXChanges
            || additionalOPEXChanges
            || cessationWellsChanges
            || cessationWellsOverrideChanges
            || cessationOffshoreFacilitiesChanges
            || cessationOffshoreFacilitiesOverrideChanges
            || cessationOnshoreFacilitiesChanges
            || surfChanges
            || surfOverrideChanges
            || substructureChanges
            || substructureOverrideChanges
            || topsideChanges
            || topsideOverrideChanges
            || transportChanges
            || transportOverrideChanges
            || oilProducerChanges
            || oilProducerOverrideChanges
            || gasProducerChanges
            || gasProducerOverrideChanges
            || waterInjectorChanges
            || waterInjectorOverrideChanges
            || gasInjectorChanges
            || gasInjectorOverrideChanges
            || gAndGAdminChanges
            || gAndGAdminOverrideChanges
            || seismicChanges
            || countryOfficeChanges
            || explorationWellChanges
            || appraisalWellChanges
            || sidetrackChanges;
    }

    private bool CalculateNPV()
    {
        var projectChanges = ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                (e.Property(nameof(Project.DiscountRate)).IsModified ||
                e.Property(nameof(Project.ExchangeRateUSDToNOK)).IsModified ||
                e.Property(nameof(Project.OilPriceUSD)).IsModified ||
                e.Property(nameof(Project.GasPriceNOK)).IsModified));

        var caseChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Case.DG4Date)).IsModified);

        return projectChanges
            || caseChanges;

    }

    private bool CalculateBreakEvenOilPrice()
    {
        var projectChanges = ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                (e.Property(nameof(Project.DiscountRate)).IsModified ||
                e.Property(nameof(Project.ExchangeRateUSDToNOK)).IsModified ||
                e.Property(nameof(Project.OilPriceUSD)).IsModified ||
                e.Property(nameof(Project.GasPriceNOK)).IsModified));

        var caseChanges = ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                e.Property(nameof(Case.DG4Date)).IsModified);

        return projectChanges
            || caseChanges;
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<RevisionDetails> RevisionDetails { get; set; } = null!;
    public DbSet<ExplorationOperationalWellCosts> ExplorationOperationalWellCosts { get; set; } = null!;
    public DbSet<DevelopmentOperationalWellCosts> DevelopmentOperationalWellCosts { get; set; } = null!;

    public DbSet<Case> Cases { get; set; } = null!;
    public DbSet<CessationWellsCostOverride> CessationWellsCostOverride { get; set; } = null!;
    public DbSet<CessationOffshoreFacilitiesCostOverride> CessationOffshoreFacilitiesCostOverride { get; set; } = null!;
    public DbSet<CessationOnshoreFacilitiesCostProfile> CessationOnshoreFacilitiesCostProfile { get; set; } = null!;
    public DbSet<TotalFeasibilityAndConceptStudiesOverride> TotalFeasibilityAndConceptStudiesOverride { get; set; } = null!;
    public DbSet<TotalFEEDStudiesOverride> TotalFEEDStudiesOverride { get; set; } = null!;
    public DbSet<TotalOtherStudiesCostProfile> TotalOtherStudiesCostProfile { get; set; } = null!;
    public DbSet<HistoricCostCostProfile> HistoricCostCostProfile { get; set; } = null!;
    public DbSet<WellInterventionCostProfileOverride> WellInterventionCostProfileOverride { get; set; } = null!;
    public DbSet<OffshoreFacilitiesOperationsCostProfileOverride> OffshoreFacilitiesOperationsCostProfileOverride { get; set; } = null!;
    public DbSet<OnshoreRelatedOPEXCostProfile> OnshoreRelatedOPEXCostProfile { get; set; } = null!;
    public DbSet<AdditionalOPEXCostProfile> AdditionalOPEXCostProfile { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;

    public DbSet<Well> Wells { get; set; } = null!;
    public DbSet<WellProjectWell> WellProjectWell { get; set; } = null!;
    public DbSet<ExplorationWell> ExplorationWell { get; set; } = null!;

    public DbSet<Surf> Surfs { get; set; } = null!;
    public DbSet<SurfCostProfile> SurfCostProfile { get; set; } = null!;
    public DbSet<SurfCostProfileOverride> SurfCostProfileOverride { get; set; } = null!;
    public DbSet<SurfCessationCostProfile> SurfCessationCostProfiles { get; set; } = null!;

    public DbSet<Substructure> Substructures { get; set; } = null!;
    public DbSet<SubstructureCostProfile> SubstructureCostProfiles { get; set; } = null!;
    public DbSet<SubstructureCostProfileOverride> SubstructureCostProfileOverride { get; set; } = null!;
    public DbSet<SubstructureCessationCostProfile> SubstructureCessationCostProfiles { get; set; } = null!;

    public DbSet<Topside> Topsides { get; set; } = null!;
    public DbSet<TopsideCostProfile> TopsideCostProfiles { get; set; } = null!;
    public DbSet<TopsideCostProfileOverride> TopsideCostProfileOverride { get; set; } = null!;
    public DbSet<TopsideCessationCostProfile> TopsideCessationCostProfiles { get; set; } = null!;

    public DbSet<Transport> Transports { get; set; } = null!;
    public DbSet<TransportCostProfile> TransportCostProfile { get; set; } = null!;
    public DbSet<TransportCostProfileOverride> TransportCostProfileOverride { get; set; } = null!;
    public DbSet<TransportCessationCostProfile> TransportCessationCostProfiles { get; set; } = null!;

    public DbSet<DrainageStrategy> DrainageStrategies { get; set; } = null!;
    public DbSet<ProductionProfileOil> ProductionProfileOil { get; set; } = null!;
    public DbSet<AdditionalProductionProfileOil> AdditionalProductionProfileOil { get; set; } = null!;

    public DbSet<ProductionProfileGas> ProductionProfileGas { get; set; } = null!;
    public DbSet<AdditionalProductionProfileGas> AdditionalProductionProfileGas { get; set; } = null!;

    public DbSet<ProductionProfileWater> ProductionProfileWater { get; set; } = null!;
    public DbSet<ProductionProfileWaterInjection> ProductionProfileWaterInjection { get; set; } = null!;

    public DbSet<FuelFlaringAndLosses> FuelFlaringAndLosses { get; set; } = null!;
    public DbSet<FuelFlaringAndLossesOverride> FuelFlaringAndLossesOverride { get; set; } = null!;

    public DbSet<NetSalesGas> NetSalesGas { get; set; } = null!;
    public DbSet<NetSalesGasOverride> NetSalesGasOverride { get; set; } = null!;

    public DbSet<Co2Emissions> Co2Emissions { get; set; } = null!;
    public DbSet<Co2EmissionsOverride> Co2EmissionsOverride { get; set; } = null!;

    public DbSet<ProductionProfileNGL> ProductionProfileNGL { get; set; } = null!;

    public DbSet<ImportedElectricity> ImportedElectricity { get; set; } = null!;
    public DbSet<ImportedElectricityOverride> ImportedElectricityOverride { get; set; } = null!;

    public DbSet<DeferredOilProduction> DeferredOilProduction { get; set; } = null!;
    public DbSet<DeferredGasProduction> DeferredGasProduction { get; set; } = null!;

    public DbSet<WellProject> WellProjects { get; set; } = null!;
    public DbSet<OilProducerCostProfileOverride> OilProducerCostProfileOverride { get; set; } = null!;
    public DbSet<GasProducerCostProfileOverride> GasProducerCostProfileOverride { get; set; } = null!;
    public DbSet<WaterInjectorCostProfileOverride> WaterInjectorCostProfileOverride { get; set; } = null!;
    public DbSet<GasInjectorCostProfileOverride> GasInjectorCostProfileOverride { get; set; } = null!;

    public DbSet<DrillingSchedule> DrillingSchedule { get; set; } = null!;

    public DbSet<Exploration> Explorations { get; set; } = null!;
    public DbSet<GAndGAdminCost> GAndGAdminCost { get; set; } = null!;
    public DbSet<SeismicAcquisitionAndProcessing> SeismicAcquisitionAndProcessing { get; set; } = null!;
    public DbSet<CountryOfficeCost> CountryOfficeCost { get; set; } = null!;
    public DbSet<GAndGAdminCostOverride> GAndGAdminCostOverride { get; set; } = null!;
    public DbSet<ExplorationWellCostProfile> ExplorationWellCostProfile { get; set; } = null!;
    public DbSet<AppraisalWellCostProfile> AppraisalWellCostProfile { get; set; } = null!;
    public DbSet<SidetrackCostProfile> SidetrackCostProfile { get; set; } = null!;

    public DbSet<CalculatedTotalIncomeCostProfile> CalculatedTotalIncomeCostProfile { get; set; } = null!;
    public DbSet<CalculatedTotalCostCostProfile> CalculatedTotalCostCostProfile { get; set; } = null!;

    public DbSet<ChangeLog> ChangeLogs { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration());
        modelBuilder.ApplyConfiguration(new RevisionDetailsConfiguration());
        modelBuilder.ApplyConfiguration(new CaseConfiguration());
        modelBuilder.ApplyConfiguration(new WellProjectWellConfiguration());
        modelBuilder.ApplyConfiguration(new ExplorationWellConfiguration());
        modelBuilder.ApplyConfiguration(new ChangeLogConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DcdEnvironments.EnableVerboseEntityFrameworkLogging)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }

        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, _currentUser, utcNow));

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        var utcNow = DateTime.UtcNow;

        ChangeLogs.AddRange(ChangeLogService.GenerateChangeLogs(this, _currentUser, utcNow));

        return base.SaveChanges();
    }
}
