using api.Context;
using api.Features.Profiles;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationDeterminerService(DcdDbContext context)
{
    public bool CalculateExplorationAndWellProjectCost()
    {
        var modifiedWellsWithCostChange = context.ChangeTracker.Entries<Well>()
            .Where(e => e.State == EntityState.Modified
                        && (e.Property(nameof(Well.WellCost)).IsModified || e.Property(nameof(Well.WellCategory)).IsModified));

        var modifiedWellIds = modifiedWellsWithCostChange.Select(e => e.Entity).ToList();

        var modifiedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Modified
                        && (e.Property(nameof(DrillingSchedule.InternalData)).IsModified
                            || e.Property(nameof(DrillingSchedule.StartYear)).IsModified));

        var addedDrillingSchedules = context.ChangeTracker.Entries<DrillingSchedule>()
            .Where(e => e.State == EntityState.Added);

        var modifiedDrillingScheduleIds = modifiedDrillingSchedules.Select(e => e.Entity.Id)
            .Union(addedDrillingSchedules.Select(e => e.Entity.Id)).ToList();

        return modifiedWellIds.Any() || modifiedDrillingScheduleIds.Any();
    }

    public bool CalculateCo2Emissions()
    {
        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = TopsideIsChanged();

        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);
        var productionProfileWaterInjection = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileWaterInjection);

        var wellChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingSchedule = DrillingScheduleIsChangedOrAdded();

        return caseItemChanges
               || topsideChanges
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas
               || productionProfileWaterInjection
               || wellChanges
               || drillingSchedule;
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
        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DevelopmentOperationalWellCosts.AnnualWellInterventionCostPerWell)).IsModified);

        var wellsChanges = context.ChangeTracker.Entries<Well>()
            .Any(e => e.State == EntityState.Modified);

        var drillingSchedule = DrillingScheduleIsChangedOrAdded();

        var topsideOpexChanges = context.ChangeTracker.Entries<Topside>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Topside.FacilityOpex)).IsModified);

        var historicCostChanges = ProfileIsChangedOrAdded(ProfileTypes.HistoricCostCostProfile);
        var onshoreRelatedOpexCostProfile = ProfileIsChangedOrAdded(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        var additionalOpexCostProfile = ProfileIsChangedOrAdded(ProfileTypes.AdditionalOPEXCostProfile);
        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);

        return developmentOperationalWellCostsChanges
               || wellsChanges
               || drillingSchedule
               || topsideOpexChanges
               || historicCostChanges
               || onshoreRelatedOpexCostProfile
               || additionalOpexCostProfile
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas;
    }

    public bool CalculateNetSalesGas()
    {
        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var caseItemChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = TopsideIsChanged();

        var drainageStrategyChanges = context.ChangeTracker.Entries<DrainageStrategy>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrainageStrategy.GasSolution)).IsModified);

        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var productionProfileWaterInjection = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileWaterInjection);

        return projectChanges
               || caseItemChanges
               || topsideChanges
               || drainageStrategyChanges
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || productionProfileWaterInjection;
    }

    public bool CalculateImportedElectricity()
    {
        var facilitiesAvailabilityChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var topsideChanges = TopsideIsChanged();

        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);
        var productionProfileWaterInjection = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileWaterInjection);

        return facilitiesAvailabilityChanges
               || topsideChanges
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas
               || productionProfileWaterInjection;
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

        var drillingSchedule = DrillingScheduleIsChangedOrAdded();

        return projectChanges || caseChanges || wellsChanges || drillingSchedule;
    }

    public bool CalculateFuelFlaringAndLosses()
    {
        var caseChanges = context.ChangeTracker.Entries<Case>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Case.FacilitiesAvailability)).IsModified);

        var projectChanges = context.ChangeTracker.Entries<Project>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Project.FlaredGasPerProducedVolume)).IsModified);

        var topsideChanges = TopsideIsChanged();

        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);
        var productionProfileWaterInjection = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileWaterInjection);

        return caseChanges
               || projectChanges
               || topsideChanges
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas
               || productionProfileWaterInjection;
    }

    public bool CalculateCessationCostProfile()
    {
        var surfChanges = context.ChangeTracker.Entries<Surf>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(Surf.CessationCost)).IsModified);

        var developmentOperationalWellCostsChanges = context.ChangeTracker.Entries<DevelopmentOperationalWellCosts>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DevelopmentOperationalWellCosts.PluggingAndAbandonment)).IsModified);

        var drillingSchedule = DrillingScheduleIsChangedOrAdded();

        var cessationWellsCostOverride = ProfileIsChangedOrAdded(ProfileTypes.CessationWellsCostOverride);
        var cessationOffshoreFacilitiesCostOverride = ProfileIsChangedOrAdded(ProfileTypes.CessationOffshoreFacilitiesCostOverride);
        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);

        return surfChanges
               || developmentOperationalWellCostsChanges
               || drillingSchedule
               || cessationWellsCostOverride
               || cessationOffshoreFacilitiesCostOverride
               || productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas;
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

        var substructureCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.SubstructureCostProfileOverride);
        var surfCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.SurfCostProfileOverride);
        var topsideCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.TopsideCostProfileOverride);
        var transportCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.TransportCostProfileOverride);
        var onshorePowerSupplyCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.OnshorePowerSupplyCostProfileOverride);
        var oilProducerCostProfile = ProfileIsChangedOrAdded(ProfileTypes.OilProducerCostProfile);
        var oilProducerCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.OilProducerCostProfileOverride);
        var gasProducerCostProfile = ProfileIsChangedOrAdded(ProfileTypes.GasProducerCostProfile);
        var gasProducerCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.GasProducerCostProfileOverride);
        var waterInjectorCostProfile = ProfileIsChangedOrAdded(ProfileTypes.WaterInjectorCostProfile);
        var waterInjectorCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.WaterInjectorCostProfileOverride);
        var gasInjectorCostProfile = ProfileIsChangedOrAdded(ProfileTypes.GasInjectorCostProfile);
        var gasInjectorCostProfileOverride = ProfileIsChangedOrAdded(ProfileTypes.GasInjectorCostProfileOverride);

        return caseChanges
               || substructureCostProfileOverride
               || surfCostProfileOverride
               || topsideCostProfileOverride
               || transportCostProfileOverride
               || onshorePowerSupplyCostProfileOverride
               || oilProducerCostProfile
               || oilProducerCostProfileOverride
               || gasProducerCostProfile
               || gasProducerCostProfileOverride
               || waterInjectorCostProfile
               || waterInjectorCostProfileOverride
               || gasInjectorCostProfile
               || gasInjectorCostProfileOverride;
    }

    public bool CalculateTotalIncome()
    {
        var productionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOil = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGas = ProfileIsChangedOrAdded(ProfileTypes.AdditionalProductionProfileGas);

        return productionProfileOil
               || additionalProductionProfileOil
               || productionProfileGas
               || additionalProductionProfileGas;
    }

    public bool CalculateTotalCost()
    {
        var totalFeasibilityAndConceptStudies = ProfileIsAdded(ProfileTypes.TotalFeasibilityAndConceptStudies);
        var totalFeedStudies = ProfileIsAdded(ProfileTypes.TotalFEEDStudies);
        var totalFeedStudiesOverride = ProfileIsAdded(ProfileTypes.TotalFEEDStudiesOverride);
        var totalOtherStudiesCostProfile = ProfileIsAdded(ProfileTypes.TotalOtherStudiesCostProfile);
        var historicCostCostProfile = ProfileIsAdded(ProfileTypes.HistoricCostCostProfile);
        var wellInterventionCostProfile = ProfileIsAdded(ProfileTypes.WellInterventionCostProfile);
        var wellInterventionCostProfileOverride = ProfileIsAdded(ProfileTypes.WellInterventionCostProfileOverride);
        var offshoreFacilitiesOperationsCostProfile = ProfileIsAdded(ProfileTypes.OffshoreFacilitiesOperationsCostProfile);
        var onshoreRelatedOpexCostProfile = ProfileIsAdded(ProfileTypes.OnshoreRelatedOPEXCostProfile);
        var additionalOpexCostProfile = ProfileIsAdded(ProfileTypes.AdditionalOPEXCostProfile);
        var cessationWellsCost = ProfileIsAdded(ProfileTypes.CessationWellsCost);
        var cessationWellsCostOverride = ProfileIsAdded(ProfileTypes.CessationWellsCostOverride);
        var cessationOffshoreFacilitiesCost = ProfileIsAdded(ProfileTypes.CessationOffshoreFacilitiesCost);
        var cessationOffshoreFacilitiesCostOverride = ProfileIsAdded(ProfileTypes.CessationOffshoreFacilitiesCostOverride);
        var cessationOnshoreFacilitiesCostProfile = ProfileIsAdded(ProfileTypes.CessationOnshoreFacilitiesCostProfile);
        var surfCostProfile = ProfileIsAdded(ProfileTypes.SurfCostProfile);
        var surfCostProfileOverride = ProfileIsAdded(ProfileTypes.SurfCostProfileOverride);
        var substructureCostProfile = ProfileIsAdded(ProfileTypes.SubstructureCostProfile);
        var substructureCostProfileOverride = ProfileIsAdded(ProfileTypes.SubstructureCostProfileOverride);
        var topsideCostProfile = ProfileIsAdded(ProfileTypes.TopsideCostProfile);
        var topsideCostProfileOverride = ProfileIsAdded(ProfileTypes.TopsideCostProfileOverride);
        var transportCostProfile = ProfileIsAdded(ProfileTypes.TransportCostProfile);
        var transportCostProfileOverride = ProfileIsAdded(ProfileTypes.TransportCostProfileOverride);
        var onshorePowerSupplyCostProfile = ProfileIsAdded(ProfileTypes.OnshorePowerSupplyCostProfile);
        var onshorePowerSupplyCostProfileOverride = ProfileIsAdded(ProfileTypes.OnshorePowerSupplyCostProfileOverride);
        var oilProducerCostProfile = ProfileIsAdded(ProfileTypes.OilProducerCostProfile);
        var oilProducerCostProfileOverride = ProfileIsAdded(ProfileTypes.OilProducerCostProfileOverride);
        var gasProducerCostProfile = ProfileIsAdded(ProfileTypes.GasProducerCostProfile);
        var gasProducerCostProfileOverride = ProfileIsAdded(ProfileTypes.GasProducerCostProfileOverride);
        var waterInjectorCostProfile = ProfileIsAdded(ProfileTypes.WaterInjectorCostProfile);
        var waterInjectorCostProfileOverride = ProfileIsAdded(ProfileTypes.WaterInjectorCostProfileOverride);
        var gasInjectorCostProfile = ProfileIsAdded(ProfileTypes.GasInjectorCostProfile);
        var gasInjectorCostProfileOverride = ProfileIsAdded(ProfileTypes.GasInjectorCostProfileOverride);
        var gAndGAdminCost = ProfileIsAdded(ProfileTypes.GAndGAdminCost);
        var gAndGAdminCostOverride = ProfileIsAdded(ProfileTypes.GAndGAdminCostOverride);
        var seismicAcquisitionAndProcessing = ProfileIsAdded(ProfileTypes.SeismicAcquisitionAndProcessing);
        var countryOfficeCost = ProfileIsAdded(ProfileTypes.CountryOfficeCost);
        var explorationWellCostProfile = ProfileIsAdded(ProfileTypes.ExplorationWellCostProfile);
        var appraisalWellCostProfile = ProfileIsAdded(ProfileTypes.AppraisalWellCostProfile);
        var sidetrackCostProfile = ProfileIsAdded(ProfileTypes.SidetrackCostProfile);

        return
            totalFeasibilityAndConceptStudies
            || totalFeedStudies
            || totalFeedStudiesOverride
            || totalOtherStudiesCostProfile
            || historicCostCostProfile
            || wellInterventionCostProfile
            || wellInterventionCostProfileOverride
            || offshoreFacilitiesOperationsCostProfile
            || onshoreRelatedOpexCostProfile
            || additionalOpexCostProfile
            || cessationWellsCost
            || cessationWellsCostOverride
            || cessationOffshoreFacilitiesCost
            || cessationOffshoreFacilitiesCostOverride
            || cessationOnshoreFacilitiesCostProfile
            || surfCostProfile
            || surfCostProfileOverride
            || substructureCostProfile
            || substructureCostProfileOverride
            || topsideCostProfile
            || topsideCostProfileOverride
            || transportCostProfile
            || transportCostProfileOverride
            || onshorePowerSupplyCostProfile
            || onshorePowerSupplyCostProfileOverride
            || oilProducerCostProfile
            || oilProducerCostProfileOverride
            || gasProducerCostProfile
            || gasProducerCostProfileOverride
            || waterInjectorCostProfile
            || waterInjectorCostProfileOverride
            || gasInjectorCostProfile
            || gasInjectorCostProfileOverride
            || gAndGAdminCost
            || gAndGAdminCostOverride
            || seismicAcquisitionAndProcessing
            || countryOfficeCost
            || explorationWellCostProfile
            || appraisalWellCostProfile
            || sidetrackCostProfile;
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

        return projectChanges || caseChanges;
    }

    private bool ProfileIsChangedOrAdded(string profileName)
    {
        var changes = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == profileName)
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(TimeSeriesProfile.StartYear)).IsModified
                      || e.Property(nameof(TimeSeriesProfile.InternalData)).IsModified);

        var added = context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == profileName)
            .Any(e => e.State == EntityState.Added);

        return changes || added;
    }

    private bool ProfileIsAdded(string profileName)
    {
        return context.ChangeTracker.Entries<TimeSeriesProfile>()
            .Where(x => x.Entity.ProfileType == profileName)
            .Any(e => e.State == EntityState.Added);
    }

    private bool TopsideIsChanged()
    {
        return context.ChangeTracker.Entries<Topside>()
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
    }

    private bool DrillingScheduleIsChangedOrAdded()
    {
        var drillingScheduleChanges = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Modified &&
                      e.Property(nameof(DrillingSchedule.InternalData)).IsModified);

        var drillingScheduleAdded = context.ChangeTracker.Entries<DrillingSchedule>()
            .Any(e => e.State == EntityState.Added);

        return drillingScheduleChanges || drillingScheduleAdded;
    }
}
