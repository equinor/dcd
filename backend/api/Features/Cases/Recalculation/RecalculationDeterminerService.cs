using api.Context;
using api.Features.Profiles;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationDeterminerService(DcdDbContext context)
{
    public (List<Well> wells, List<Guid> drillingScheduleIds) CalculateExplorationAndWellProjectCost()
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

    public bool CalculateCo2Emissions()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
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

    public bool CalculateCo2Intensity()
    {
        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        return caseItemChanges;
    }

    public bool CalculateOpex()
    {
        var historicCostChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.HistoricCostCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified);

        var historicCostAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.HistoricCostCostProfile)
            .Any(e => e.State == EntityState.Added);

        var onshoreOpexChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshoreRelatedOPEXCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified);

        var onshoreOpexAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshoreRelatedOPEXCostProfile)
            .Any(e => e.State == EntityState.Added);

        var additionalOpexChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalOPEXCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified);

        var additionalOpexAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalOPEXCostProfile)
            .Any(e => e.State == EntityState.Added);

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
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

    public bool CalculateNetSalesGas()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
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

    public bool CalculateImportedElectricity()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
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

    public bool CalculateGAndGAdminCost()
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

    public bool CalculateFuelFlaringAndLosses()
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

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                          e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                          e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var productionProfileWaterInjectionChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                          e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileWaterInjectionAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileWaterInjection)
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

    public bool CalculateCessationCostProfile()
    {
        var caseCessationWellsCostOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationWellsCostOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                      ));

        var cessationWellsCostAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationWellsCostOverride)
            .Any(e => e.State == EntityState.Added);

        var caseCessationOffshoreFacilitiesCostOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationOffshoreFacilitiesCostOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                      ));

        var cessationOffshoreFacilitiesCostAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationOffshoreFacilitiesCostOverride)
            .Any(e => e.State == EntityState.Added);

        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
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

    public bool CalculateStudyCost()
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

        var substructureChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SubstructureCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var substructureCostProfileAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SubstructureCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SurfCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var surfCostProfileAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SurfCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TopsideCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var topsideCostProfileAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TopsideCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TransportCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var transportCostProfileAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TransportCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshorePowerSupplyCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var onshorePowerSupplyAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshorePowerSupplyCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectOilProducerAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfile)
            .Any(e => e.State == EntityState.Added);

        var wellProjectOilProducerOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectOilProducerOverrideAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectGasProducerAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfile)
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasProducerOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectGasProducerOverrideAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfile)
            .Any(e => e.State == EntityState.Added);

        var wellProjectWaterInjectorOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectWaterInjectorOverrideAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfileOverride)
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfile)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfile)
            .Any(e => e.State == EntityState.Added);

        var wellProjectGasInjectorOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfileOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var wellProjectGasInjectorOverrideAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfileOverride)
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

    public bool CalculateTotalIncome()
    {
        var productionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                          e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileOilChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileOilAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileOil)
            .Any(e => e.State == EntityState.Added);

        var productionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                          e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      ));

        var productionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ProductionProfileGas)
            .Any(e => e.State == EntityState.Added);

        var additionalProductionProfileGasChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
            .Any(e => e.State == EntityState.Modified &&
                      (e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified ||
                       e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified));

        var additionalProductionProfileGasAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalProductionProfileGas)
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

    public bool CalculateTotalCost()
    {

        var totalFeasibilityAdded = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TotalFeasibilityAndConceptStudies)
            .Any(e => e.State == EntityState.Added);

        var totalFeasibilityOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)
            .Any(e => e.State == EntityState.Modified &&
                      (
                          e.Property(nameof(TimeSeriesProfile.Override)).IsModified
                          || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified
                      ));

        var totalFeedChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TotalFEEDStudies)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalFeedOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TotalFEEDStudiesOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var totalOtherStudiesChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TotalOtherStudiesCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var historicCostChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.HistoricCostCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WellInterventionCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var wellInterventionOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WellInterventionCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OffshoreFacilitiesOperationsCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var offshoreFacilitiesOperationsOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OffshoreFacilitiesOperationsCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshoreRelatedOpexChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshoreRelatedOPEXCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var additionalOpexChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AdditionalOPEXCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationWellsCost)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationWellsOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationWellsCostOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationOffshoreFacilitiesCost)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOffshoreFacilitiesOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationOffshoreFacilitiesCost)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var cessationOnshoreFacilitiesChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CessationOnshoreFacilitiesCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SurfCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var surfOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SurfCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SubstructureCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var substructureOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SubstructureCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TopsideCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var topsideOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TopsideCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TransportCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var transportOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.TransportCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshorePowerSupplyCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var onshorePowerSupplyOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OnshorePowerSupplyCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var oilProducerOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.OilProducerCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasProducerOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasProducerCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var waterInjectorOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.WaterInjectorCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gasInjectorOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GasInjectorCostProfileOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GAndGAdminCostOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var gAndGAdminOverrideChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.GAndGAdminCostOverride)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var seismicChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SeismicAcquisitionAndProcessing)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var countryOfficeChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.CountryOfficeCost)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var explorationWellChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.ExplorationWellCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var appraisalWellChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.AppraisalWellCostProfile)
            .Any(e => e.State is EntityState.Modified or EntityState.Added);

        var sidetrackChanges = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == ProfileTypes.SidetrackCostProfile)
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

    public bool CalculateNpv()
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

    public bool CalculateBreakEvenOilPrice()
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
