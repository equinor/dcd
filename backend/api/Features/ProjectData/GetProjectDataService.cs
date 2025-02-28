using api.Features.ProjectAccess;
using api.Features.ProjectData.Dtos;

namespace api.Features.ProjectData;

public class GetProjectDataService(GetProjectDataRepository getProjectDataRepository, UserActionsService userActionsService)
{
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        var projectPk = await getProjectDataRepository.GetPrimaryKeyForProjectId(projectId);

        var projectMembers = await getProjectDataRepository.GetProjectMembers(projectPk);
        var revisionDetailsList = await getProjectDataRepository.GetRevisionDetailsList(projectPk);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(projectPk);

        var userActions = await userActionsService.CalculateActionsForProject(projectPk);

        return new ProjectDataDto
        {
            ProjectId = projectPk,
            DataType = "project",
            UserActions = userActions,
            ProjectMembers = projectMembers,
            RevisionDetailsList = revisionDetailsList,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    public async Task<RevisionDataDto> GetRevisionData(Guid revisionId)
    {
        var originalProjectId = await getProjectDataRepository.GetOriginalProjectIdForRevision(revisionId);
        var revisionDetailsList = await getProjectDataRepository.GetRevisionDetailsList(originalProjectId);

        var revisionDetails = await getProjectDataRepository.GetRevisionDetails(revisionId);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(revisionId);

        var userActions = await userActionsService.CalculateActionsForRevision(revisionId);

        return new RevisionDataDto
        {
            ProjectId = originalProjectId,
            RevisionId = revisionId,
            DataType = "revision",
            UserActions = userActions,
            RevisionDetails = revisionDetails,
            RevisionDetailsList = revisionDetailsList,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    private async Task<CommonProjectAndRevisionDto> LoadCommonProjectAndRevisionData(Guid projectId)
    {
        var commonProjectAndRevisionData = await getProjectDataRepository.GetCommonProjectAndRevisionData(projectId);

        commonProjectAndRevisionData.Cases = await getProjectDataRepository.GetCases(projectId);
        commonProjectAndRevisionData.Wells = await getProjectDataRepository.GetWells(projectId);
        commonProjectAndRevisionData.UpdatedUtc = commonProjectAndRevisionData.Cases.Select(c => c.UpdatedUtc).Append(commonProjectAndRevisionData.UpdatedUtc).Max();

        return commonProjectAndRevisionData;
    }
}
