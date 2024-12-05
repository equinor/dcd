using api.AppInfrastructure.Authorization;
using api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Explorations.Repositories;
using api.Features.Assets.CaseAssets.Explorations.Services;
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
using api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;
using api.Features.Cases.CaseComparison;
using api.Features.Cases.Create;
using api.Features.Cases.Delete;
using api.Features.Cases.Duplicate;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Update;
using api.Features.FusionIntegration.OrgChart;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Images.Service;
using api.Features.ProjectAccess;
using api.Features.ProjectData;
using api.Features.ProjectMembers.Create;
using api.Features.ProjectMembers.Delete;
using api.Features.ProjectMembers.Get;
using api.Features.ProjectMembers.Update;
using api.Features.Projects.Create;
using api.Features.Projects.Update;
using api.Features.Prosp.Services;
using api.Features.Revisions.Create;
using api.Features.Revisions.Get;
using api.Features.Revisions.Update;
using api.Features.Stea;
using api.Features.TechnicalInput;
using api.Features.Wells.Create;
using api.Features.Wells.Delete;
using api.Features.Wells.Get;
using api.Features.Wells.GetAffectedCases;
using api.Features.Wells.Update;
using api.ModelMapping;

using Microsoft.AspNetCore.Authorization;

namespace api.AppInfrastructure;

public static class DcdIocConfiguration
{
    public static void AddDcdIocConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IProjectAccessService, ProjectAccessService>();

        services.AddScoped<CreateProjectService>();
        services.AddScoped<UpdateProjectService>();

        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<FusionOrgChartMemberService>();

        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<CreateCaseService>();
        services.AddScoped<UpdateCaseService>();
        services.AddScoped<DeleteCaseService>();

        services.AddScoped<IDrainageStrategyService, DrainageStrategyService>();
        services.AddScoped<IWellProjectService, WellProjectService>();
        services.AddScoped<IExplorationService, ExplorationService>();
        services.AddScoped<ISurfService, SurfService>();
        services.AddScoped<ISubstructureService, SubstructureService>();
        services.AddScoped<ITopsideService, TopsideService>();
        services.AddScoped<ITransportService, TransportService>();

        services.AddScoped<ICostProfileFromDrillingScheduleHelper, CostProfileFromDrillingScheduleHelper>();

        services.AddScoped<ICaseTimeSeriesService, CaseTimeSeriesService>();
        services.AddScoped<IDrainageStrategyTimeSeriesService, DrainageStrategyTimeSeriesService>();
        services.AddScoped<IWellProjectTimeSeriesService, WellProjectTimeSeriesService>();
        services.AddScoped<IExplorationTimeSeriesService, ExplorationTimeSeriesService>();
        services.AddScoped<ISurfTimeSeriesService, SurfTimeSeriesService>();
        services.AddScoped<ISubstructureTimeSeriesService, SubstructureTimeSeriesService>();
        services.AddScoped<ITopsideTimeSeriesService, TopsideTimeSeriesService>();
        services.AddScoped<ITransportTimeSeriesService, TransportTimeSeriesService>();

        services.AddScoped<TechnicalInputService>();

        services.AddScoped<IOpexCostProfileService, OpexCostProfileService>();
        services.AddScoped<IStudyCostProfileService, StudyCostProfileService>();
        services.AddScoped<ICo2EmissionsProfileService, Co2EmissionsProfileService>();
        services.AddScoped<IGenerateGAndGAdminCostProfile, GenerateGAndGAdminCostProfile>();
        services.AddScoped<ICessationCostProfileService, CessationCostProfileService>();
        services.AddScoped<IImportedElectricityProfileService, ImportedElectricityProfileService>();
        services.AddScoped<IFuelFlaringLossesProfileService, FuelFlaringLossesProfileService>();
        services.AddScoped<INetSaleGasProfileService, NetSaleGasProfileService>();
        services.AddScoped<ICo2IntensityProfileService, Co2IntensityProfileService>();
        services.AddScoped<ICo2IntensityTotalService, Co2IntensityTotalService>();
        services.AddScoped<CaseComparisonService>();
        services.AddScoped<CaseComparisonRepository>();
        services.AddScoped<ICo2DrillingFlaringFuelTotalsService, Co2DrillingFlaringFuelTotalsService>();
        services.AddScoped<IWellCostProfileService, WellCostProfileService>();

        services.AddScoped<SteaService>();

        services.AddScoped<GetProjectDataService>();

        services.AddScoped<GetProjectMemberService>();
        services.AddScoped<DeleteProjectMemberService>();
        services.AddScoped<CreateProjectMemberService>();
        services.AddScoped<UpdateProjectMemberService>();

        services.AddScoped<DuplicateCaseService>();
        services.AddScoped<DuplicateCaseRepository>();

        services.AddScoped<GetRevisionService>();
        services.AddScoped<CreateRevisionService>();
        services.AddScoped<CreateRevisionRepository>();
        services.AddScoped<UpdateRevisionService>();

        services.AddScoped<ICaseRepository, CaseRepository>();
        services.AddScoped<ISubstructureRepository, SubstructureRepository>();
        services.AddScoped<ITopsideRepository, TopsideRepository>();
        services.AddScoped<IDrainageStrategyRepository, DrainageStrategyRepository>();
        services.AddScoped<IWellProjectRepository, WellProjectRepository>();
        services.AddScoped<IExplorationRepository, ExplorationRepository>();
        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<ISurfRepository, SurfRepository>();

        services.AddScoped<ICaseTimeSeriesRepository, CaseTimeSeriesRepository>();
        services.AddScoped<IDrainageStrategyTimeSeriesRepository, DrainageStrategyTimeSeriesRepository>();
        services.AddScoped<ISubstructureTimeSeriesRepository, SubstructureTimeSeriesRepository>();
        services.AddScoped<ITopsideTimeSeriesRepository, TopsideTimeSeriesRepository>();
        services.AddScoped<IWellProjectTimeSeriesRepository, WellProjectTimeSeriesRepository>();
        services.AddScoped<IExplorationTimeSeriesRepository, ExplorationTimeSeriesRepository>();
        services.AddScoped<ITransportTimeSeriesRepository, TransportTimeSeriesRepository>();
        services.AddScoped<ISurfTimeSeriesRepository, SurfTimeSeriesRepository>();

        services.AddScoped<GetWellService>();
        services.AddScoped<CreateWellService>();
        services.AddScoped<UpdateWellService>();
        services.AddScoped<DeleteWellService>();
        services.AddScoped<GetAffectedCasesService>();

        services.AddScoped<CaseWithAssetsRepository>();
        services.AddScoped<CaseWithAssetsService>();

        services.AddScoped<IMapperService, MapperService>();
        services.AddScoped<IConversionMapperService, ConversionMapperService>();

        services.AddScoped<ProspExcelImportService>();
        services.AddScoped<ProspSharepointImportService>();
        services.AddScoped<CurrentUser>();
        services.AddScoped<UpdateProjectFromProjectMasterService>();
        services.AddScoped<IProjectWithAssetsRepository, ProjectWithCasesRepository>();

        services.AddScoped<IAuthorizationHandler, ApplicationRoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, ApplicationRolePolicyProvider>();

        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<ICalculateBreakEvenOilPriceService, CalculateBreakEvenOilPriceService>();
        services.AddScoped<ICalculateNPVService, CalculateNPVService>();
        services.AddScoped<ICalculateTotalCostService, CalculateTotalCostService>();
        services.AddScoped<ICalculateTotalIncomeService, CalculateTotalIncomeService>();

        // Project assets
        services.AddScoped<UpdateDevelopmentOperationalWellCostsService>();
        services.AddScoped<UpdateExplorationOperationalWellCostsService>();
    }
}
