using api.AppInfrastructure.Authorization;
using api.Features.Assets.CaseAssets.Campaigns.Create;
using api.Features.Assets.CaseAssets.Campaigns.Delete;
using api.Features.Assets.CaseAssets.Campaigns.Get;
using api.Features.Assets.CaseAssets.Campaigns.Update;
using api.Features.Assets.CaseAssets.CampaignWells.Get;
using api.Features.Assets.CaseAssets.CampaignWells.Save;
using api.Features.Assets.CaseAssets.DrainageStrategies;
using api.Features.Assets.CaseAssets.DrillingSchedules;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies;
using api.Features.Assets.CaseAssets.Substructures;
using api.Features.Assets.CaseAssets.Surfs;
using api.Features.Assets.CaseAssets.Topsides;
using api.Features.Assets.CaseAssets.Transports;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;
using api.Features.BackgroundServices.ProjectMaster.Services;
using api.Features.BackgroundServices.ProjectRecalculation.Services;
using api.Features.Cases.CaseComparison;
using api.Features.Cases.Create;
using api.Features.Cases.Delete;
using api.Features.Cases.Duplicate;
using api.Features.Cases.GetWithAssets;
using api.Features.Cases.Recalculation;
using api.Features.Cases.Update;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.Images.Copy;
using api.Features.Images.Delete;
using api.Features.Images.Get;
using api.Features.Images.Update;
using api.Features.Images.Upload;
using api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;
using api.Features.Profiles.Save;
using api.Features.ProjectAccess;
using api.Features.ProjectData;
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
using api.Features.Wells.GetIsInUse;
using api.Features.Wells.Update;

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

        /* Project members */
        services.AddScoped<GetProjectMemberService>();
        services.AddScoped<DeleteProjectMemberService>();
        services.AddScoped<CreateProjectMemberService>();
        services.AddScoped<UpdateProjectMemberService>();

        /* Wells */
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

        /* Case images */
        services.AddScoped<GetCaseImageService>();
        services.AddScoped<DeleteCaseImageService>();
        services.AddScoped<UploadCaseImageService>();
        services.AddScoped<UpdateCaseImageService>();

        /* Project images */
        services.AddScoped<GetProjectImageService>();
        services.AddScoped<DeleteProjectImageService>();
        services.AddScoped<UploadProjectImageService>();
        services.AddScoped<UpdateProjectImageService>();

        services.AddScoped<CopyImageService>();

        /* Background jobs */
        services.AddScoped<UpdateProjectFromProjectMasterService>();
        services.AddScoped<RecalculateProjectService>();

        /* Project assets */
        services.AddScoped<UpdateDevelopmentOperationalWellCostsService>();
        services.AddScoped<UpdateExplorationOperationalWellCostsService>();

        /* Recalculation services */
        services.AddScoped<RecalculationService>();
        services.AddScoped<RecalculationRepository>();

        /* Auth */
        services.AddScoped<CurrentUser>();
        services.AddScoped<IAuthorizationHandler, DcdAuthorizationHandler>();

        /* Prosp / Excel import */
        services.AddScoped<ProspExcelImportService>();
        services.AddScoped<ProspSharepointImportService>();

        /* Stea / Excel export */
        services.AddScoped<SteaService>();
        services.AddScoped<SteaRepository>();

        /* Integrations / external systems */
        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<FusionOrgChartProjectMemberService>();

        /* Drilling campaigns */
        services.AddScoped<UpdateCampaignService>();
        services.AddScoped<CreateCampaignService>();
        services.AddScoped<SaveCampaignWellService>();
        services.AddScoped<GetCampaignWellService>();
        services.AddScoped<GetCampaignService>();
        services.AddScoped<DeleteCampaignService>();

        /* Case assets */
        services.AddScoped<UpdateDrainageStrategyService>();
        services.AddScoped<UpdateOnshorePowerSupplyService>();
        services.AddScoped<UpdateSubstructureService>();
        services.AddScoped<UpdateSurfService>();
        services.AddScoped<UpdateTopsideService>();
        services.AddScoped<UpdateTransportService>();

        /* Drilling schedules */
        services.AddScoped<DrillingScheduleService>();

        /* Time series profiles */
        services.AddScoped<SaveProfileService>();

        services.AddScoped<Co2DrillingFlaringFuelTotalsService>();
    }
}
