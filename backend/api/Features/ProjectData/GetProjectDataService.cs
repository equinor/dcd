using api.Features.ProjectData.Dtos;

namespace api.Features.ProjectData;

public class GetProjectDataService(GetProjectDataRepository getProjectDataRepository)
{
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        var projectPk = await getProjectDataRepository.GetProjectIdFromFusionId(projectId);

        var projectMembers = await getProjectDataRepository.GetProjectMembers(projectPk);
        var revisionDetailsList = await getProjectDataRepository.GetRevisionDetailsList(projectPk);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(projectPk);

        return new ProjectDataDto
        {
            ProjectId = projectPk,
            ProjectMembers = projectMembers,
            RevisionDetailsList = revisionDetailsList.OrderBy(x => x.RevisionDate).ToList(),
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    public async Task<RevisionDataDto> GetRevisionData(Guid projectId, Guid revisionId)
    {
        var revisionDetails = await getProjectDataRepository.GetRevisionDetails(revisionId);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(revisionId);

        return new RevisionDataDto
        {
            ProjectId = projectId,
            RevisionId = revisionId,
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
        commonProjectAndRevisionData.DrainageStrategies = await getProjectDataRepository.GetDrainageStrategies(projectId);

        commonProjectAndRevisionData.ModifyTime = commonProjectAndRevisionData.Cases.Select(c => c.ModifyTime).Append(commonProjectAndRevisionData.ModifyTime).Max();

        return commonProjectAndRevisionData;
    }
}
