using System.Diagnostics;

using api.Context;
using api.Features.CaseGeneratedProfiles.GenerateCo2Intensity;
using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Cases.Recalculation.Types.CessationCostProfile;
using api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;
using api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;
using api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;
using api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;
using api.Features.Cases.Recalculation.Types.NetSaleGasProfile;
using api.Features.Cases.Recalculation.Types.OpexCostProfile;
using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Features.Cases.Recalculation.Types.WellCostProfile;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationService(DcdDbContext context, IServiceProvider serviceProvider) : IRecalculationService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public async Task<Dictionary<string, long>> RunAllRecalculations(Guid caseId)
    {
        Dictionary<string, long> debugLog = [];

        var stopwatch = Stopwatch.StartNew();

        var (wells, drillingScheduleIds) = CalculateExplorationAndWellProjectCost();
        debugLog.Add("CalculateExplorationAndWellProjectCost", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        if (wells.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            debugLog.Add("UpdateCostProfilesForWellsFromDrillingSchedules", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWells(wells);
            debugLog.Add("UpdateCostProfilesForWells", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
        }

        await serviceProvider.GetRequiredService<StudyCostProfileService>().Generate(caseId);
        debugLog.Add("StudyCostProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<CessationCostProfileService>().Generate(caseId);
        debugLog.Add("CessationCostProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<FuelFlaringLossesProfileService>().Generate(caseId);
        debugLog.Add("FuelFlaringLossesProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>().Generate(caseId);
        debugLog.Add("GenerateGAndGAdminCostProfile", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<ImportedElectricityProfileService>().Generate(caseId);
        debugLog.Add("ImportedElectricityProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<NetSaleGasProfileService>().Generate(caseId);
        debugLog.Add("NetSaleGasProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<OpexCostProfileService>().Generate(caseId);
        debugLog.Add("OpexCostProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<Co2EmissionsProfileService>().Generate(caseId);
        debugLog.Add("Co2EmissionsProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<Co2IntensityProfileService>().Generate(caseId);
        debugLog.Add("Co2IntensityProfileService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<CalculateTotalIncomeService>().CalculateTotalIncome(caseId);
        debugLog.Add("CalculateTotalIncomeService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<CalculateTotalCostService>().CalculateTotalCost(caseId);
        debugLog.Add("CalculateTotalCostService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<CalculateNpvService>().CalculateNpv(caseId);
        debugLog.Add("CalculateNpvService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await serviceProvider.GetRequiredService<CalculateBreakEvenOilPriceService>().CalculateBreakEvenOilPrice(caseId);
        debugLog.Add("CalculateBreakEvenOilPriceService", stopwatch.ElapsedMilliseconds);
        stopwatch.Restart();

        await context.SaveChangesAsync();
        debugLog.Add("SaveChangesAsync", stopwatch.ElapsedMilliseconds);

        return debugLog;
    }

    public async Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        await Semaphore.WaitAsync(cancellationToken);
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
                    result = await context.SaveChangesAsync(cancellationToken);
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
            Semaphore.Release();
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
        var rerunCo2Intensity = CalculateCo2Intensity();
        var rerunTotalIncome = CalculateTotalIncome();
        var rerunTotalCost = CalculateTotalCost();
        var rerunCalculateNpv = CalculateNpv();
        var rerunCalculateBreakEven = CalculateBreakEvenOilPrice();

        await context.SaveChangesAsync(); // TODO: This is a hack to find the updated values in the calculate services. Need to find a better way to do this.
        if (wells.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWells(wells);
        }
        if (rerunStudyCost)
        {
            await serviceProvider.GetRequiredService<StudyCostProfileService>().Generate(caseId);
        }

        if (rerunCessationCostProfile)
        {
            await serviceProvider.GetRequiredService<CessationCostProfileService>().Generate(caseId);
        }

        if (rerunFuelFlaringAndLosses)
        {
            await serviceProvider.GetRequiredService<FuelFlaringLossesProfileService>().Generate(caseId);
        }

        if (rerunGAndGAdminCost)
        {
            await serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>().Generate(caseId);
        }

        if (rerunImportedElectricity)
        {
            await serviceProvider.GetRequiredService<ImportedElectricityProfileService>().Generate(caseId);
        }

        if (rerunNetSalesGas)
        {
            await serviceProvider.GetRequiredService<NetSaleGasProfileService>().Generate(caseId);
        }

        if (rerunOpex)
        {
            await serviceProvider.GetRequiredService<OpexCostProfileService>().Generate(caseId);
        }

        if (rerunCo2Emissions)
        {
            await serviceProvider.GetRequiredService<Co2EmissionsProfileService>().Generate(caseId);
        }

        if (rerunCo2Intensity)
        {
            await serviceProvider.GetRequiredService<Co2IntensityProfileService>().Generate(caseId);
        }

        if (rerunTotalIncome)
        {
            var calculateIncomeHelper = serviceProvider.GetRequiredService<CalculateTotalIncomeService>();
            await calculateIncomeHelper.CalculateTotalIncome(caseId);
        }

        if (rerunTotalCost)
        {
            var calculateCostHelper = serviceProvider.GetRequiredService<CalculateTotalCostService>();
            await calculateCostHelper.CalculateTotalCost(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateNpv)
        {
            var calculateNpvHelper = serviceProvider.GetRequiredService<CalculateNpvService>();
            await calculateNpvHelper.CalculateNpv(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateBreakEven)
        {
            var calculateBreakEvenHelper = serviceProvider.GetRequiredService<CalculateBreakEvenOilPriceService>();
            await calculateBreakEvenHelper.CalculateBreakEvenOilPrice(caseId);
        }
    }

    private (List<Well> wells, List<Guid> drillingScheduleIds) CalculateExplorationAndWellProjectCost()
    {
        var modifiedWellsWithCostChange = context.ChangeTracker.Entries<Well>()
            .Where(e => (e.State == EntityState.Modified)
                        && (e.Property(nameof(Well.WellCost)).IsModified || e.Property(nameof(Well.WellCategory)).IsModified));

        var modifiedWellIds = modifiedWellsWithCostChange.Select(e => e.Entity).ToList();

        var modifiedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => (e.State == EntityState.Modified)
                        && (e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                            || e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var addedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Added);

        var modifiedDrillingScheduleIds = modifiedDrillingSchedules.Select(e => e.Entity.Id)
            .Union(addedDrillingSchedules.Select(e => e.Entity.Id)).ToList();

        return (modifiedWellIds, modifiedDrillingScheduleIds);
    }

    private bool CalculateCo2Emissions()
    {
        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var wellChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
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

    private bool CalculateCo2Intensity()
    {
        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var wellChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
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
        var historicCostChanges = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(HistoricCostCostProfile.StartYear)).IsModified
                      || e.Property(nameof(HistoricCostCostProfile.InternalData)).IsModified);

        var historicCostAdded = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var onshoreOpexChanges = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(OnshoreRelatedOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(OnshoreRelatedOPEXCostProfile.InternalData)).IsModified);

        var onshoreOpexAdded = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var additionalOpexChanges = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(AdditionalOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(AdditionalOPEXCostProfile.InternalData)).IsModified);

        var additionalOpexAdded = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.StartYear)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.InternalData)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var wellsChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        var topsideOpexChanges = context.ChangeTracker.Entries<Topside>()
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
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Added);

        var drainageStrategyChanges = context.ChangeTracker.Entries<DrainageStrategy>()
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
        var facilitiesAvailabilityChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileOil.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
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
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.Country)).IsModified);

        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Case.DG4Date)).IsModified ||
                       e.Property(nameof(Case.DG1Date)).IsModified));

        var wellsChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                      || e.Property(nameof(DrillingSchedule.StartYear)).IsModified);

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return projectChanges || caseChanges || wellsChanges || drillingScheduleChanges || drillingScheduleAdded;
    }

    private bool CalculateFuelFlaringAndLosses()
    {
        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var topsideChanges = context.ChangeTracker.Entries<Topside>()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileOil.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileGas.StartYear)).IsModified
                      ));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileWaterInjection.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileWaterInjection.StartYear)).IsModified
                      ));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<ProductionProfileWaterInjection>()
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
        var caseCessationWellsCostOverrideChanges = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(CessationWellsCostOverride.InternalData)).IsModified
                          || e.Property(nameof(CessationWellsCostOverride.Override)).IsModified
                      ));

        var cessationWellsCostAdded = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var caseCessationOffshoreFacilitiesCostOverrideChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(CessationOffshoreFacilitiesCostOverride.InternalData)).IsModified
                          || e.Property(nameof(CessationOffshoreFacilitiesCostOverride.Override)).IsModified
                      ));

        var cessationOffshoreFacilitiesCostAdded = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileOil.InternalData)).IsModified
                          || e.Property(nameof(ProductionProfileOil.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(ProductionProfileGas.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<Surf>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(Surf.CessationCost)).IsModified
                      ));

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(DevelopmentOperationalWellCosts.PluggingAndAbandonment)).IsModified
                      ));
        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                      ));

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
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
        var caseChanges = context.ChangeTracker.Entries<Case>()
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

        var substructureChanges = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(SubstructureCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(SubstructureCostProfileOverride.InternalData)).IsModified
                      ));

        var substructureCostProfileAdded = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(SurfCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(SurfCostProfileOverride.InternalData)).IsModified
                      ));

        var surfCostProfileAdded = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TopsideCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(TopsideCostProfileOverride.InternalData)).IsModified
                      ));

        var topsideCostProfileAdded = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TransportCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(TransportCostProfileOverride.InternalData)).IsModified
                      ));

        var transportCostProfileAdded = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OnshorePowerSupplyCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(OnshorePowerSupplyCostProfileOverride.InternalData)).IsModified
                      ));

        var onshorePowerSupplyAdded = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerChanges = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OilProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectOilProducerAdded = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerOverrideChanges = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(OilProducerCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(OilProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectOilProducerOverrideAdded = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerChanges = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasProducerAdded = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerOverrideChanges = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasProducerCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(GasProducerCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasProducerOverrideAdded = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorChanges = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(WaterInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorAdded = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorOverrideChanges = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(WaterInjectorCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(WaterInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorOverrideAdded = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorChanges = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorAdded = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorOverrideChanges = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(GasInjectorCostProfileOverride.Override)).IsModified
                          || e.Property(nameof(GasInjectorCostProfileOverride.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorOverrideAdded = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
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
               || onshorePowerSupplyChanges
               || onshorePowerSupplyAdded
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
        var productionProfileOilChanges = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileOil.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileOil.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);


        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileOil.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileOil.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<AdditionalProductionProfileOil>()
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(ProductionProfileGas.InternalData)).IsModified ||
                          e.Property(nameof(ProductionProfileGas.StartYear)).IsModified
                      ));

        var productionProfileGasAdded = context.ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(AdditionalProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(AdditionalProductionProfileGas.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<AdditionalProductionProfileGas>()
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

        var totalFeasibilityAdded = context.ChangeTracker.Entries<TotalFeasibilityAndConceptStudies>()
            .Any(e => e.State == EntityState.Added);

        var totalFeasibilityOverrideChanges = context.ChangeTracker.Entries<TotalFeasibilityAndConceptStudiesOverride>()
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TotalFeasibilityAndConceptStudiesOverride.Override)).IsModified
                          || e.Property(nameof(TotalFeasibilityAndConceptStudiesOverride.InternalData)).IsModified
                      ));


        var totalFeedChanges = context.ChangeTracker.Entries<TotalFEEDStudies>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalFeedOverrideChanges = context.ChangeTracker.Entries<TotalFEEDStudiesOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalOtherStudiesChanges = context.ChangeTracker.Entries<TotalOtherStudiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var historicCostChanges = context.ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionChanges = context.ChangeTracker.Entries<WellInterventionCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionOverrideChanges = context.ChangeTracker.Entries<WellInterventionCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsChanges = context.ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsOverrideChanges = context.ChangeTracker.Entries<OffshoreFacilitiesOperationsCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshoreRelatedOpexChanges = context.ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var additionalOpexChanges = context.ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsChanges = context.ChangeTracker.Entries<CessationWellsCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsOverrideChanges = context.ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesOverrideChanges = context.ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOnshoreFacilitiesChanges = context.ChangeTracker.Entries<CessationOnshoreFacilitiesCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<SurfCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfOverrideChanges = context.ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureChanges = context.ChangeTracker.Entries<SubstructureCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureOverrideChanges = context.ChangeTracker.Entries<SubstructureCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TopsideCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideOverrideChanges = context.ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TransportCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportOverrideChanges = context.ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyOverrideChanges = context.ChangeTracker.Entries<OnshorePowerSupplyCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerChanges = context.ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerOverrideChanges = context.ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerChanges = context.ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerOverrideChanges = context.ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorChanges = context.ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorOverrideChanges = context.ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorChanges = context.ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorOverrideChanges = context.ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminChanges = context.ChangeTracker.Entries<GAndGAdminCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminOverrideChanges = context.ChangeTracker.Entries<GAndGAdminCostOverride>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var seismicChanges = context.ChangeTracker.Entries<SeismicAcquisitionAndProcessing>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var countryOfficeChanges = context.ChangeTracker.Entries<CountryOfficeCost>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var explorationWellChanges = context.ChangeTracker.Entries<ExplorationWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var appraisalWellChanges = context.ChangeTracker.Entries<AppraisalWellCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var sidetrackChanges = context.ChangeTracker.Entries<SidetrackCostProfile>()
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        return
            totalFeasibilityAdded
            || totalFeasibilityOverrideChanges
            || totalFeedChanges
            || totalFeedOverrideChanges
            || totalOtherStudiesChanges
            || historicCostChanges
            || wellInterventionChanges
            || wellInterventionOverrideChanges
            || offshoreFacilitiesOperationsChanges
            || offshoreFacilitiesOperationsOverrideChanges
            || onshoreRelatedOpexChanges
            || additionalOpexChanges
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
            || onshorePowerSupplyChanges
            || onshorePowerSupplyOverrideChanges
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

    private bool CalculateNpv()
    {
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Project.DiscountRate)).IsModified ||
                       e.Property(nameof(Project.ExchangeRateUSDToNOK)).IsModified ||
                       e.Property(nameof(Project.OilPriceUSD)).IsModified ||
                       e.Property(nameof(Project.GasPriceNOK)).IsModified));

        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.DG4Date)).IsModified);

        return projectChanges
               || caseChanges;

    }

    private bool CalculateBreakEvenOilPrice()
    {
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Project.DiscountRate)).IsModified ||
                       e.Property(nameof(Project.ExchangeRateUSDToNOK)).IsModified ||
                       e.Property(nameof(Project.OilPriceUSD)).IsModified ||
                       e.Property(nameof(Project.GasPriceNOK)).IsModified));

        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.DG4Date)).IsModified);

        return projectChanges
               || caseChanges;
    }
}
