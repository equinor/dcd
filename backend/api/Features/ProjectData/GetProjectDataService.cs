using api.Features.ProjectAccess.V2;
using api.Features.ProjectData.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
using api.Models;

namespace api.Features.ProjectData;

public class GetProjectDataService(GetProjectDataRepository getProjectDataRepository, ProjectAccessServiceV2 projectAccessService)
{
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        var projectPk = await getProjectDataRepository.GetPrimaryKeyForProjectId(projectId);

        var hasAccess = await projectAccessService.UserHasViewAccessToProject(projectPk);

        if (!hasAccess)
        {
            var projectClassification = await getProjectDataRepository.GetProjectClassification(projectPk);
            return EmptyProjectDto(projectPk, projectClassification);
        }

        var projectMembers = await getProjectDataRepository.GetProjectMembers(projectPk);
        var revisionDetailsList = await getProjectDataRepository.GetRevisionDetailsList(projectPk);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(projectPk);

        return new ProjectDataDto
        {
            ProjectId = projectPk,
            DataType = "project",
            HasAccess = hasAccess,
            ProjectMembers = projectMembers,
            RevisionDetailsList = revisionDetailsList,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    public async Task<RevisionDataDto> GetRevisionData(Guid projectId, Guid revisionId)
    {
        var hasAccess = await projectAccessService.UserHasViewAccessToRevision(projectId, revisionId);

        if (!hasAccess)
        {
            var projectClassification = await getProjectDataRepository.GetProjectClassification(projectId, revisionId);
            return EmptyRevisionDto(projectId, revisionId, projectClassification);
        }

        var revisionDetails = await getProjectDataRepository.GetRevisionDetails(revisionId);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(revisionId);

        return new RevisionDataDto
        {
            ProjectId = projectId,
            RevisionId = revisionId,
            DataType = "revision",
            HasAccess = hasAccess,
            RevisionDetails = revisionDetails,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    private async Task<CommonProjectAndRevisionDto> LoadCommonProjectAndRevisionData(Guid projectId)
    {
        var commonProjectAndRevisionData = await getProjectDataRepository.GetCommonProjectAndRevisionData(projectId);

        commonProjectAndRevisionData.Cases = await getProjectDataRepository.GetCases(projectId);
        commonProjectAndRevisionData.Wells = await getProjectDataRepository.GetWells(projectId);
        commonProjectAndRevisionData.Surfs = await getProjectDataRepository.GetSurfs(projectId);
        commonProjectAndRevisionData.Substructures = await getProjectDataRepository.GetSubstructures(projectId);
        commonProjectAndRevisionData.Topsides = await getProjectDataRepository.GetTopsides(projectId);
        commonProjectAndRevisionData.Transports = await getProjectDataRepository.GetTransports(projectId);
        commonProjectAndRevisionData.OnshorePowerSupplies = await getProjectDataRepository.GetOnshorePowerSupplies(projectId);
        commonProjectAndRevisionData.DrainageStrategies = await getProjectDataRepository.GetDrainageStrategies(projectId);
        commonProjectAndRevisionData.ModifyTime = commonProjectAndRevisionData.Cases.Select(c => c.ModifyTime).Append(commonProjectAndRevisionData.ModifyTime).Max();

        return commonProjectAndRevisionData;
    }

    private static ProjectDataDto EmptyProjectDto(Guid projectId, ProjectClassification projectClassification)
    {
        return new ProjectDataDto
        {
            ProjectId = projectId,
            DataType = "project",
            HasAccess = false,
            ProjectMembers = [],
            RevisionDetailsList = [],
            CommonProjectAndRevisionData = DefaultCommonProjectAndRevisionData(projectClassification)
        };
    }

    private static RevisionDataDto EmptyRevisionDto(Guid projectId, Guid revisionId, ProjectClassification projectClassification)
    {
        return new RevisionDataDto
        {
            ProjectId = projectId,
            RevisionId = revisionId,
            DataType = "revision",
            HasAccess = false,
            RevisionDetails = new RevisionDetailsDto
            {
                RevisionId = revisionId,
                RevisionName = null,
                RevisionDate = default,
                Arena = false,
                Mdqc = false
            },
            CommonProjectAndRevisionData = DefaultCommonProjectAndRevisionData(projectClassification)
        };
    }

    private static CommonProjectAndRevisionDto DefaultCommonProjectAndRevisionData(ProjectClassification projectClassification)
    {
        return new CommonProjectAndRevisionDto
        {
            ModifyTime = default,
            Classification = projectClassification,
            Name = "-",
            FusionProjectId = Guid.Empty,
            ReferenceCaseId = Guid.Empty,
            Description = "-",
            Country = "-",
            Currency = 0,
            PhysicalUnit = PhysUnit.SI,
            ProjectPhase = ProjectPhase.Null,
            InternalProjectPhase = InternalProjectPhase.APbo,
            ProjectCategory = ProjectCategory.Null,
            SharepointSiteUrl = null,
            CO2RemovedFromGas = 0,
            CO2EmissionFromFuelGas = 0,
            FlaredGasPerProducedVolume = 0,
            CO2EmissionsFromFlaredGas = 0,
            CO2Vented = 0,
            DailyEmissionFromDrillingRig = 0,
            AverageDevelopmentDrillingDays = 0,
            OilPriceUSD = 0,
            GasPriceNOK = 0,
            DiscountRate = 0,
            ExchangeRateUSDToNOK = 0,
            ExplorationOperationalWellCosts = new ExplorationOperationalWellCostsOverviewDto
            {
                ExplorationRigUpgrading = 0,
                ExplorationRigMobDemob = 0,
                ExplorationProjectDrillingCosts = 0,
                AppraisalRigMobDemob = 0,
                AppraisalProjectDrillingCosts = 0
            },
            DevelopmentOperationalWellCosts = new DevelopmentOperationalWellCostsOverviewDto
            {
                RigUpgrading = 0,
                RigMobDemob = 0,
                AnnualWellInterventionCostPerWell = 0,
                PluggingAndAbandonment = 0
            },
            Cases = [],
            Wells = [],
            Surfs = [],
            Substructures = [],
            Topsides = [],
            Transports = [],
            OnshorePowerSupplies = [],
            DrainageStrategies = []
        };
    }
}
