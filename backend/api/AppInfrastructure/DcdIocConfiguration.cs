using api.AppInfrastructure.Authorization;
using api.Features.Assets.CaseAssets.DrainageStrategies;
using api.Features.Assets.CaseAssets.Explorations;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies;
using api.Features.Assets.CaseAssets.Substructures;
using api.Features.Assets.CaseAssets.Surfs;
using api.Features.Assets.CaseAssets.Topsides;
using api.Features.Assets.CaseAssets.Transports;
using api.Features.Assets.CaseAssets.WellProjects;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;
using api.Features.BackgroundServices.ProjectMaster.Services;
using api.Features.BackgroundServices.ProjectRecalculation.Services;
using api.Features.CaseGeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;
using api.Features.CaseGeneratedProfiles.GenerateCo2Intensity;
using api.Features.Cases.CaseComparison;
using api.Features.Cases.Create;
using api.Features.Cases.Delete;
using api.Features.Cases.Duplicate;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
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
using api.Features.Cases.Update;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Images.Copy;
using api.Features.Images.Delete;
using api.Features.Images.Get;
using api.Features.Images.Update;
using api.Features.Images.Upload;
using api.Features.Profiles.Cases.AdditionalOpexCostProfiles;
using api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides;
using api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles;
using api.Features.Profiles.Cases.CessationWellsCostOverrides;
using api.Features.Profiles.Cases.HistoricCostCostProfiles;
using api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides;
using api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles;
using api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides;
using api.Features.Profiles.Cases.TotalFeedStudiesOverrides;
using api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles;
using api.Features.Profiles.Cases.WellInterventionCostProfileOverrides;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils;
using api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions;
using api.Features.Profiles.DrainageStrategies.DeferredOilProductions;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides;
using api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaters;
using api.Features.Profiles.Explorations.CountryOfficeCosts;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides;
using api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfileOverrides;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfiles;
using api.Features.Profiles.Substructures.SubstructureCostProfileOverrides;
using api.Features.Profiles.Substructures.SubstructureCostProfiles;
using api.Features.Profiles.Surfs.SurfCostProfileOverrides;
using api.Features.Profiles.Surfs.SurfCostProfiles;
using api.Features.Profiles.Topsides.TopsideCostProfileOverrides;
using api.Features.Profiles.Topsides.TopsideCostProfiles;
using api.Features.Profiles.Transports.TransportCostProfileOverrides;
using api.Features.Profiles.Transports.TransportCostProfiles;
using api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides;
using api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides;
using api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides;
using api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides;
using api.Features.ProjectAccess;
using api.Features.ProjectData;
using api.Features.ProjectIntegrity;
using api.Features.ProjectMembers.Create;
using api.Features.ProjectMembers.Delete;
using api.Features.ProjectMembers.Get;
using api.Features.ProjectMembers.Get.Sync;
using api.Features.ProjectMembers.Update;
using api.Features.Projects.Create;
using api.Features.Projects.Exists;
using api.Features.Projects.Update;
using api.Features.Prosp.Services;
using api.Features.Revisions.Create;
using api.Features.Revisions.Update;
using api.Features.Stea;
using api.Features.TechnicalInput;
using api.Features.Wells.Create;
using api.Features.Wells.Delete;
using api.Features.Wells.Get;
using api.Features.Wells.GetIsInUse;
using api.Features.Wells.Update;
using api.ModelMapping;

using Microsoft.AspNetCore.Authorization;

namespace api.AppInfrastructure;

public static class DcdIocConfiguration
{
    public static void AddDcdIocConfiguration(this IServiceCollection services)
    {
        /* Projects / revisions */
        services.AddScoped<GetProjectDataService>();
        services.AddScoped<GetProjectDataRepository>();
        services.AddScoped<CreateProjectService>();
        services.AddScoped<UpdateProjectService>();
        services.AddScoped<UserActionsService>();
        services.AddScoped<ProjectExistsService>();

        services.AddScoped<CreateRevisionService>();
        services.AddScoped<CreateRevisionRepository>();
        services.AddScoped<UpdateRevisionService>();

        services.AddScoped<UpdateWellsService>();
        services.AddScoped<UpdateExplorationWellCostProfilesService>();
        services.AddScoped<UpdateWellProjectCostProfilesService>();

        /* Project members */
        services.AddScoped<GetProjectMemberService>();
        services.AddScoped<DeleteProjectMemberService>();
        services.AddScoped<CreateProjectMemberService>();
        services.AddScoped<UpdateProjectMemberService>();

        /* Wells */
        services.AddScoped<GetWellService>();
        services.AddScoped<CreateWellService>();
        services.AddScoped<UpdateWellService>();
        services.AddScoped<DeleteWellService>();
        services.AddScoped<GetIsWellInUseService>();

        /* Cases */
        services.AddScoped<CreateCaseService>();
        services.AddScoped<UpdateCaseService>();
        services.AddScoped<DeleteCaseService>();

        services.AddScoped<DuplicateCaseService>();
        services.AddScoped<DuplicateCaseRepository>();

        services.AddScoped<CaseComparisonService>();
        services.AddScoped<CaseComparisonRepository>();

        services.AddScoped<CaseWithAssetsService>();
        services.AddScoped<CaseWithAssetsRepository>();

        /* Images */
        services.AddScoped<GetImageService>();
        services.AddScoped<DeleteImageService>();
        services.AddScoped<UploadImageService>();
        services.AddScoped<CopyImageService>();
        services.AddScoped<UpdateImageService>();

        /* Mapping */
        services.AddScoped<IMapperService, MapperService>();
        services.AddScoped<IConversionMapperService, ConversionMapperService>();

        /* Background jobs */
        services.AddScoped<UpdateProjectFromProjectMasterService>();
        services.AddScoped<RecalculateProjectService>();

        /* Project assets */
        services.AddScoped<UpdateDevelopmentOperationalWellCostsService>();
        services.AddScoped<UpdateExplorationOperationalWellCostsService>();

        /* Recalculation services */
        services.AddScoped<IRecalculationService, RecalculationService>();
        services.AddScoped<WellCostProfileService>();
        services.AddScoped<StudyCostProfileService>();
        services.AddScoped<CessationCostProfileService>();
        services.AddScoped<FuelFlaringLossesProfileService>();
        services.AddScoped<GenerateGAndGAdminCostProfile>();
        services.AddScoped<ImportedElectricityProfileService>();
        services.AddScoped<NetSaleGasProfileService>();
        services.AddScoped<OpexCostProfileService>();
        services.AddScoped<Co2EmissionsProfileService>();
        services.AddScoped<Co2IntensityProfileService>();
        services.AddScoped<CalculateTotalIncomeService>();
        services.AddScoped<CalculateTotalCostService>();
        services.AddScoped<CalculateNpvService>();
        services.AddScoped<CalculateBreakEvenOilPriceService>();

        /* Auth */
        services.AddScoped<CurrentUser>();
        services.AddScoped<IAuthorizationHandler, DcdAuthorizationHandler>();
        services.AddScoped<IProjectIntegrityService, ProjectIntegrityService>();

        /* Prosp / Excel import */
        services.AddScoped<ProspExcelImportService>();
        services.AddScoped<ProspSharepointImportService>();

        /* Stea / Excel export */
        services.AddScoped<SteaService>();
        services.AddScoped<SteaRepository>();

        /* Integrations / external systems */
        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<FusionOrgChartProjectMemberService>();

        /* Case assets */
        services.AddScoped<UpdateDrainageStrategyService>();
        services.AddScoped<UpdateExplorationService>();
        services.AddScoped<UpdateOnshorePowerSupplyService>();
        services.AddScoped<UpdateSubstructureService>();
        services.AddScoped<UpdateSurfService>();
        services.AddScoped<UpdateTopsideService>();
        services.AddScoped<UpdateTransportService>();
        services.AddScoped<UpdateWellProjectService>();

        /* Case profiles */
        services.AddScoped<AdditionalOpexCostProfileService>();
        services.AddScoped<CessationOffshoreFacilitiesCostOverrideService>();
        services.AddScoped<CessationOnshoreFacilitiesCostProfileService>();
        services.AddScoped<CessationWellsCostOverrideService>();
        services.AddScoped<HistoricCostCostProfileService>();
        services.AddScoped<OffshoreFacilitiesOperationsCostProfileOverrideService>();
        services.AddScoped<OnshoreRelatedOpexCostProfileService>();
        services.AddScoped<TotalFeasibilityAndConceptStudiesOverrideService>();
        services.AddScoped<TotalFeedStudiesOverrideService>();
        services.AddScoped<TotalOtherStudiesCostProfileService>();
        services.AddScoped<WellInterventionCostProfileOverrideService>();

        /* Drainage strategy profiles */
        services.AddScoped<AdditionalProductionProfileGasService>();
        services.AddScoped<AdditionalProductionProfileOilService>();
        services.AddScoped<Co2EmissionsOverrideService>();
        services.AddScoped<DeferredGasProductionService>();
        services.AddScoped<DeferredOilProductionService>();
        services.AddScoped<FuelFlaringAndLossesOverrideService>();
        services.AddScoped<ImportedElectricityOverrideService>();
        services.AddScoped<NetSalesGasOverrideService>();
        services.AddScoped<ProductionProfileGasService>();
        services.AddScoped<ProductionProfileOilService>();
        services.AddScoped<ProductionProfileWaterInjectionService>();
        services.AddScoped<ProductionProfileWaterService>();

        /* Exploration profiles */
        services.AddScoped<CountryOfficeCostService>();
        services.AddScoped<GAndGAdminCostOverrideService>();
        services.AddScoped<SeismicAcquisitionAndProcessingService>();

        /* Onshore power supply profiles */
        services.AddScoped<OnshorePowerSupplyCostProfileService>();
        services.AddScoped<OnshorePowerSupplyTimeSeriesService>();

        /* Substructure profiles */
        services.AddScoped<SubstructureCostProfileService>();
        services.AddScoped<SubstructureCostProfileOverrideService>();

        /* Surf profiles */
        services.AddScoped<SurfCostProfileService>();
        services.AddScoped<SurfTimeSeriesService>();

        /* Topside profiles */
        services.AddScoped<TopsideCostProfileService>();
        services.AddScoped<TopsideCostProfileOverrideService>();

        /* Transport profiles */
        services.AddScoped<TransportCostProfileService>();
        services.AddScoped<TransportCostProfileOverrideService>();

        /* Well project profiles */
        services.AddScoped<GasInjectorCostProfileOverrideService>();
        services.AddScoped<GasProducerCostProfileOverrideService>();
        services.AddScoped<OilProducerCostProfileOverrideService>();
        services.AddScoped<WaterInjectorCostProfileOverrideService>();

        services.AddScoped<Co2DrillingFlaringFuelTotalsService>();
    }
}
