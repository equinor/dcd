using api.AppInfrastructure.Authorization;
using api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Explorations.Repositories;
using api.Features.Assets.CaseAssets.Explorations.Services;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Repositories;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;
using api.Features.Assets.CaseAssets.Substructures.Repositories;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.Assets.CaseAssets.Surfs.Repositories;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.Assets.CaseAssets.Topsides.Repositories;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.Assets.CaseAssets.Transports.Repositories;
using api.Features.Assets.CaseAssets.Transports.Services;
using api.Features.Assets.CaseAssets.WellProjects.Repositories;
using api.Features.Assets.CaseAssets.WellProjects.Services;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;
using api.Features.BackgroundServices.ProjectMaster.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.CaseProfiles.Services;
using api.Features.CaseProfiles.Services.GenerateCostProfiles;
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
using api.Features.Images.Service;
using api.Features.ProjectAccess;
using api.Features.ProjectData;
using api.Features.ProjectMembers.Create;
using api.Features.ProjectMembers.Delete;
using api.Features.ProjectMembers.Get;
using api.Features.ProjectMembers.Get.Sync;
using api.Features.ProjectMembers.Update;
using api.Features.Projects.Create;
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

        services.AddScoped<CreateRevisionService>();
        services.AddScoped<CreateRevisionRepository>();
        services.AddScoped<UpdateRevisionService>();

        services.AddScoped<TechnicalInputService>();

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
        services.AddScoped<IWellCostProfileService, WellCostProfileService>();
        services.AddScoped<IStudyCostProfileService, StudyCostProfileService>();
        services.AddScoped<ICessationCostProfileService, CessationCostProfileService>();
        services.AddScoped<IFuelFlaringLossesProfileService, FuelFlaringLossesProfileService>();
        services.AddScoped<IGenerateGAndGAdminCostProfile, GenerateGAndGAdminCostProfile>();
        services.AddScoped<IImportedElectricityProfileService, ImportedElectricityProfileService>();
        services.AddScoped<INetSaleGasProfileService, NetSaleGasProfileService>();
        services.AddScoped<IOpexCostProfileService, OpexCostProfileService>();
        services.AddScoped<ICo2EmissionsProfileService, Co2EmissionsProfileService>();
        services.AddScoped<ICalculateTotalIncomeService, CalculateTotalIncomeService>();
        services.AddScoped<ICalculateTotalCostService, CalculateTotalCostService>();
        services.AddScoped<ICalculateNpvService, CalculateNpvService>();
        services.AddScoped<ICalculateBreakEvenOilPriceService, CalculateBreakEvenOilPriceService>();

        /* Auth */
        services.AddScoped<CurrentUser>();
        services.AddScoped<IAuthorizationHandler, ApplicationRoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, ApplicationRolePolicyProvider>();
        services.AddScoped<IProjectAccessService, ProjectAccessService>();

        /* Prosp / Excel import */
        services.AddScoped<ProspExcelImportService>();
        services.AddScoped<ProspSharepointImportService>();

        /* Stea / Excel export */
        services.AddScoped<SteaService>();

        /* Integrations / external systems */
        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<FusionOrgChartProjectMemberService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        /* Misc */
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IProjectWithAssetsRepository, ProjectWithCasesRepository>();

        services.AddScoped<IDrainageStrategyService, DrainageStrategyService>();
        services.AddScoped<IWellProjectService, WellProjectService>();
        services.AddScoped<IExplorationService, ExplorationService>();
        services.AddScoped<ISurfService, SurfService>();
        services.AddScoped<ISubstructureService, SubstructureService>();
        services.AddScoped<ITopsideService, TopsideService>();
        services.AddScoped<ITransportService, TransportService>();
        services.AddScoped<IOnshorePowerSupplyService, OnshorePowerSupplyService>();

        services.AddScoped<ICostProfileFromDrillingScheduleHelper, CostProfileFromDrillingScheduleHelper>();

        services.AddScoped<ICaseTimeSeriesService, CaseTimeSeriesService>();
        services.AddScoped<IDrainageStrategyTimeSeriesService, DrainageStrategyTimeSeriesService>();
        services.AddScoped<IWellProjectTimeSeriesService, WellProjectTimeSeriesService>();
        services.AddScoped<IExplorationTimeSeriesService, ExplorationTimeSeriesService>();
        services.AddScoped<ISurfTimeSeriesService, SurfTimeSeriesService>();
        services.AddScoped<ISubstructureTimeSeriesService, SubstructureTimeSeriesService>();
        services.AddScoped<ITopsideTimeSeriesService, TopsideTimeSeriesService>();
        services.AddScoped<ITransportTimeSeriesService, TransportTimeSeriesService>();
        services.AddScoped<IOnshorePowerSupplyTimeSeriesService, OnshorePowerSupplyTimeSeriesService>();

        services.AddScoped<ICo2IntensityProfileService, Co2IntensityProfileService>();
        services.AddScoped<ICo2IntensityTotalService, Co2IntensityTotalService>();
        services.AddScoped<ICo2DrillingFlaringFuelTotalsService, Co2DrillingFlaringFuelTotalsService>();

        services.AddScoped<ICaseRepository, CaseRepository>();
        services.AddScoped<ISubstructureRepository, SubstructureRepository>();
        services.AddScoped<ITopsideRepository, TopsideRepository>();
        services.AddScoped<IDrainageStrategyRepository, DrainageStrategyRepository>();
        services.AddScoped<IWellProjectRepository, WellProjectRepository>();
        services.AddScoped<IExplorationRepository, ExplorationRepository>();
        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<ISurfRepository, SurfRepository>();
        services.AddScoped<IOnshorePowerSupplyRepository, OnshorePowerSupplyRepository>();

        services.AddScoped<ICaseTimeSeriesRepository, CaseTimeSeriesRepository>();
        services.AddScoped<IDrainageStrategyTimeSeriesRepository, DrainageStrategyTimeSeriesRepository>();
        services.AddScoped<ISubstructureTimeSeriesRepository, SubstructureTimeSeriesRepository>();
        services.AddScoped<ITopsideTimeSeriesRepository, TopsideTimeSeriesRepository>();
        services.AddScoped<IWellProjectTimeSeriesRepository, WellProjectTimeSeriesRepository>();
        services.AddScoped<IExplorationTimeSeriesRepository, ExplorationTimeSeriesRepository>();
        services.AddScoped<ITransportTimeSeriesRepository, TransportTimeSeriesRepository>();
        services.AddScoped<ISurfTimeSeriesRepository, SurfTimeSeriesRepository>();
        services.AddScoped<IOnshorePowerSupplyTimeSeriesRepository, OnshorePowerSupplyTimeSeriesRepository>();
    }
}
