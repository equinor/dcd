using api.AppInfrastructure.Authorization;
using api.Features.BackgroundJobs;
using api.Features.Cases.CaseComparison;
using api.Features.Cases.Duplicate;
using api.Features.Cases.GetWithAssets;
using api.Features.FusionIntegration.OrgChart;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Images.Service;
using api.Features.Prosp.Services;
using api.Features.Revision.Create;
using api.Features.Revision.Get;
using api.Features.Revision.Update;
using api.Repositories;
using api.Services;
using api.Services.EconomicsServices;
using api.Services.GenerateCostProfiles;

using Microsoft.AspNetCore.Authorization;

namespace api.AppInfrastructure;

public static class DcdIocConfiguration
{
    public static void AddDcdIocConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IOrgChartMemberService, OrgChartMemberService>();

        services.AddScoped<IProjectAccessService, ProjectAccessService>();

        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectMemberService, ProjectMemberService>();
        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<ICreateCaseService, CreateCaseService>();
        services.AddScoped<IDrainageStrategyService, DrainageStrategyService>();
        services.AddScoped<IWellProjectService, WellProjectService>();
        services.AddScoped<IExplorationService, ExplorationService>();
        services.AddScoped<ISurfService, SurfService>();
        services.AddScoped<ISubstructureService, SubstructureService>();
        services.AddScoped<ITopsideService, TopsideService>();
        services.AddScoped<ITransportService, TransportService>();

        services.AddScoped<IWellService, WellService>();
        services.AddScoped<IWellProjectWellService, WellProjectWellService>();
        services.AddScoped<IExplorationWellService, ExplorationWellService>();
        services.AddScoped<ICostProfileFromDrillingScheduleHelper, CostProfileFromDrillingScheduleHelper>();
        services.AddScoped<IExplorationOperationalWellCostsService, ExplorationOperationalWellCostsService>();

        services.AddScoped<ICaseTimeSeriesService, CaseTimeSeriesService>();
        services.AddScoped<IDrainageStrategyTimeSeriesService, DrainageStrategyTimeSeriesService>();
        services.AddScoped<IWellProjectTimeSeriesService, WellProjectTimeSeriesService>();
        services.AddScoped<IExplorationTimeSeriesService, ExplorationTimeSeriesService>();
        services.AddScoped<ISurfTimeSeriesService, SurfTimeSeriesService>();
        services.AddScoped<ISubstructureTimeSeriesService, SubstructureTimeSeriesService>();
        services.AddScoped<ITopsideTimeSeriesService, TopsideTimeSeriesService>();
        services.AddScoped<ITransportTimeSeriesService, TransportTimeSeriesService>();

        services.AddScoped<IDevelopmentOperationalWellCostsService, DevelopmentOperationalWellCostsService>();

        services.AddScoped<ITechnicalInputService, TechnicalInputService>();
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

        services.AddScoped<ISTEAService, STEAService>();

        services.AddScoped<IProjectAccessRepository, ProjectAccessRepository>();

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();

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

        services.AddScoped<IWellRepository, WellRepository>();

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
    }
}
