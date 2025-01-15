using api.AppInfrastructure.Authorization;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Explorations.Services;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.Assets.CaseAssets.Transports.Services;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;
using api.Features.BackgroundServices.ProjectMaster.Services;
using api.Features.CaseGeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;
using api.Features.CaseGeneratedProfiles.GenerateCo2Intensity;
using api.Features.CaseGeneratedProfiles.GenerateCo2IntensityTotal;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides;
using api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides;
using api.Features.CaseProfiles.Services.HistoricCostCostProfiles;
using api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides;
using api.Features.CaseProfiles.Services.OnshoreRelatedOpexCostProfiles;
using api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides;
using api.Features.CaseProfiles.Services.TotalFeedStudiesOverrides;
using api.Features.CaseProfiles.Services.TotalOtherStudiesCostProfiles;
using api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides;
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

        services.AddScoped<TechnicalInputService>();
        services.AddScoped<UpdateProjectAndOperationalWellsCostService>();
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
        services.AddScoped<DrainageStrategyService>();
        services.AddScoped<DrainageStrategyTimeSeriesService>();

        services.AddScoped<ExplorationService>();
        services.AddScoped<ExplorationTimeSeriesService>();

        services.AddScoped<OnshorePowerSupplyService>();
        services.AddScoped<OnshorePowerSupplyTimeSeriesService>();

        services.AddScoped<SubstructureService>();
        services.AddScoped<SubstructureTimeSeriesService>();

        services.AddScoped<SurfService>();
        services.AddScoped<SurfTimeSeriesService>();

        services.AddScoped<TopsideService>();
        services.AddScoped<TopsideTimeSeriesService>();

        services.AddScoped<TransportService>();
        services.AddScoped<TransportTimeSeriesService>();

        services.AddScoped<WellProjectService>();
        services.AddScoped<WellProjectTimeSeriesService>();

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


        services.AddScoped<Co2IntensityProfileService>();
        services.AddScoped<Co2IntensityTotalService>();
        services.AddScoped<Co2DrillingFlaringFuelTotalsService>();
    }
}
