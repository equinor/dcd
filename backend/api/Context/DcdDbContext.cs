using api.Models;
using api.Services;
using api.Services.GenerateCostProfiles;

using Microsoft.EntityFrameworkCore;


namespace api.Context;

public class DcdDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider = null!;

    public DcdDbContext(DbContextOptions<DcdDbContext> options) : base(options)
    {

    }

    public DcdDbContext(
        DbContextOptions<DcdDbContext> options,
        IServiceProvider serviceProvider
        ) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    // TODO: This is not pretty, need to move this logic out of the context
    public async Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        await DetectChangesAndCalculateEntities(caseId);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DetectChangesAndCalculateEntities(Guid caseId)
    {
        var (wellIds, drillingScheduleIds) = CalculateExplorationAndWellProjectCost();
        if (wellIds.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await _serviceProvider.GetRequiredService<ICostProfileFromDrillingScheduleHelper>().UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            await _serviceProvider.GetRequiredService<ICostProfileFromDrillingScheduleHelper>().UpdateCostProfilesForWells(wellIds);
        }
        {

        }
        if (CalculateStudyCost())
        {
            await _serviceProvider.GetRequiredService<IStudyCostProfileService>().Generate(caseId);
        }

        if (CalculateCessationCostProfile())
        {
            await _serviceProvider.GetRequiredService<ICessationCostProfileService>().Generate(caseId);
        }

        if (CalculateFuelFlaringAndLosses())
        {
            await _serviceProvider.GetRequiredService<IFuelFlaringLossesProfileService>().Generate(caseId);
        }

        if (CalculateGAndGAdminCost())
        {
            await _serviceProvider.GetRequiredService<IGenerateGAndGAdminCostProfile>().Generate(caseId);
        }

        if (CalculateImportedElectricity())
        {
            await _serviceProvider.GetRequiredService<IImportedElectricityProfileService>().Generate(caseId);
        }

        if (CalculateNetSalesGas())
        {
            await _serviceProvider.GetRequiredService<INetSaleGasProfileService>().Generate(caseId);
        }

        if (CalculateOpex())
        {
            await _serviceProvider.GetRequiredService<IOpexCostProfileService>().Generate(caseId);
        }

        if (CalculateCo2Emissions())
        {
            await _serviceProvider.GetRequiredService<ICo2EmissionsProfileService>().Generate(caseId);
        }
    }

    private (List<Guid> wellIds, List<Guid> drillingScheduleIds) CalculateExplorationAndWellProjectCost()
    {
        /* Well costs, drilling schedule */
        // This will fetch all entries for the Well entity that have been modified.
        var modifiedWellsWithCostChange = ChangeTracker.Entries<Well>()
            .Where(e => e.State == EntityState.Modified && e.Property(nameof(Well.WellCost)).IsModified);

        // Extract and return the IDs of these modified wells.
        var modifiedWellIds = modifiedWellsWithCostChange.Select(e => e.Entity.Id).ToList();

        var modifiedDrillingSchedules = ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Modified && e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified
            || e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified);

        var modifiedDrillingScheduleIds = modifiedDrillingSchedules.Select(e => e.Entity.Id).ToList();

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

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        var wellChanges = ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified));

        return caseItemChanges || topsideChanges || productionProfileOilChanges || productionProfileGasChanges || productionProfileWaterInjectionChanges || wellChanges || drillingScheduleChanges;
    }

    private bool CalculateOpex()
    {
        var historicCostChanges = ChangeTracker.Entries<HistoricCostCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.HistoricCostCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.HistoricCostCostProfile.InternalData)).IsModified);

        var onshoreOpexChanges = ChangeTracker.Entries<OnshoreRelatedOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.OnshoreRelatedOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.OnshoreRelatedOPEXCostProfile.InternalData)).IsModified);

        var additionalOpexChanges = ChangeTracker.Entries<AdditionalOPEXCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.AdditionalOPEXCostProfile.StartYear)).IsModified
                      || e.Property(nameof(Models.AdditionalOPEXCostProfile.InternalData)).IsModified);

        var developmentOperationalWellCostsChanges = ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Models.DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified));

        var wellsChanges = ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingScheduleChanges = ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.DrillingSchedule.InternalData)).IsModified ||
                       e.Property(nameof(Models.DrillingSchedule.StartYear)).IsModified));

        var topsideOpexChanges = ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Topside.FacilityOpex)).IsModified);

        return historicCostChanges || onshoreOpexChanges || additionalOpexChanges || developmentOperationalWellCostsChanges || productionProfileOilChanges || wellsChanges || drillingScheduleChanges || topsideOpexChanges;
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

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        var drainageStrategyChanges = ChangeTracker.Entries<DrainageStrategy>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrainageStrategy.GasSolution)).IsModified);

        return projectChanges || caseItemChanges || topsideChanges || productionProfileOilChanges || productionProfileGasChanges || productionProfileWaterInjectionChanges || drainageStrategyChanges;
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

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified));

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                       e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified));

        return facilitiesAvailabilityChanges || topsideChanges || productionProfileOilChanges || productionProfileGasChanges || productionProfileWaterInjectionChanges;
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

        return projectChanges || caseChanges || wellsChanges || drillingScheduleChanges;
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

        var productionProfileGasChanges = ChangeTracker.Entries<ProductionProfileGas>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileGas.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileGas.StartYear)).IsModified
                ));

        var productionProfileWaterInjectionChanges = ChangeTracker.Entries<ProductionProfileWaterInjection>()
            .Any(e => e.State == EntityState.Modified &&
                (
                    e.Property(nameof(Models.ProductionProfileWaterInjection.InternalData)).IsModified ||
                    e.Property(nameof(Models.ProductionProfileWaterInjection.StartYear)).IsModified
                ));

        return caseChanges || projectChanges || topsideChanges || productionProfileOilChanges
            || productionProfileGasChanges || productionProfileWaterInjectionChanges;
    }

    private bool CalculateCessationCostProfile()
    {
        var caseCessationWellsCostOverrideChanges = ChangeTracker.Entries<CessationWellsCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.CessationWellsCostOverride.InternalData)).IsModified
                || e.Property(nameof(Models.CessationWellsCostOverride.Override)).IsModified
            ));

        var caseCessationOffshoreFacilitiesCostOverrideChanges = ChangeTracker.Entries<CessationOffshoreFacilitiesCostOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.CessationOffshoreFacilitiesCostOverride.InternalData)).IsModified
                || e.Property(nameof(Models.CessationOffshoreFacilitiesCostOverride.Override)).IsModified
            ));

        var productionProfileOilChanges = ChangeTracker.Entries<ProductionProfileOil>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.ProductionProfileOil.InternalData)).IsModified
                || e.Property(nameof(Models.ProductionProfileOil.StartYear)).IsModified
            ));

        var surfChanges = ChangeTracker.Entries<SurfCessationCostProfile>()
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

        return caseCessationWellsCostOverrideChanges || caseCessationOffshoreFacilitiesCostOverrideChanges
            || productionProfileOilChanges || surfChanges || developmentOperationalWellCostsChanges || drillingScheduleChanges;
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

        var surfChanges = ChangeTracker.Entries<SurfCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.SurfCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.SurfCostProfileOverride.InternalData)).IsModified
            ));

        var topsideChanges = ChangeTracker.Entries<TopsideCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.TopsideCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.TopsideCostProfileOverride.InternalData)).IsModified
            ));
        var transportChanges = ChangeTracker.Entries<TransportCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.TransportCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.TransportCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectOilProducerChanges = ChangeTracker.Entries<OilProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.OilProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectOilProducerOverrideChanges = ChangeTracker.Entries<OilProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.OilProducerCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.OilProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasProducerChanges = ChangeTracker.Entries<GasProducerCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasProducerOverrideChanges = ChangeTracker.Entries<GasProducerCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasProducerCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.GasProducerCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectWaterInjectorChanges = ChangeTracker.Entries<WaterInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.WaterInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectWaterInjectorOverrideChanges = ChangeTracker.Entries<WaterInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.WaterInjectorCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.WaterInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasInjectorChanges = ChangeTracker.Entries<GasInjectorCostProfile>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasInjectorCostProfileOverride.InternalData)).IsModified
            ));

        var wellProjectGasInjectorOverrideChanges = ChangeTracker.Entries<GasInjectorCostProfileOverride>()
            .Any(e => e.State == EntityState.Modified &&
            (
                e.Property(nameof(Models.GasInjectorCostProfileOverride.Override)).IsModified
                || e.Property(nameof(Models.GasInjectorCostProfileOverride.InternalData)).IsModified
            ));

        return caseChanges || substructureChanges || surfChanges || topsideChanges || transportChanges
            || wellProjectOilProducerChanges || wellProjectOilProducerOverrideChanges
            || wellProjectGasProducerChanges || wellProjectGasProducerOverrideChanges
            || wellProjectWaterInjectorChanges || wellProjectWaterInjectorOverrideChanges
            || wellProjectGasInjectorChanges || wellProjectGasInjectorOverrideChanges;
    }

    public DbSet<Project> Projects { get; set; } = null!;
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
    public DbSet<ProductionProfileGas> ProductionProfileGas { get; set; } = null!;
    public DbSet<ProductionProfileOil> ProductionProfileOil { get; set; } = null!;
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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellProjectWell>()
            .HasKey(wc => new { wc.WellProjectId, wc.WellId });

        modelBuilder.Entity<WellProjectWell>()
            .HasOne(w => w.Well)
            .WithMany(w => w.WellProjectWells)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ExplorationWell>()
            .HasKey(ew => new { ew.ExplorationId, ew.WellId });

        modelBuilder.Entity<ExplorationWell>()
            .HasOne(w => w.Well)
            .WithMany(w => w.ExplorationWells)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Project>()
            .Property(p => p.Classification)
            .HasDefaultValue(ProjectClassification.Internal);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
