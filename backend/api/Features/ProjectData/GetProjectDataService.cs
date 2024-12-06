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
        var projectPk = await getProjectDataRepository.GetProjectIdForRevision(projectId, revisionId);

        var revisionDetails = await getProjectDataRepository.GetRevisionDetails(projectPk, revisionId);
        var commonProjectAndRevisionData = await LoadCommonProjectAndRevisionData(projectPk);

        return new RevisionDataDto
        {
            ProjectId = projectId,
            RevisionId = revisionId,
            RevisionDetails = revisionDetails,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }


    private async Task<CommonProjectAndRevisionDto> LoadCommonProjectAndRevisionData(Guid projectPk)
    {
        var commonProjectAndRevisionData = await getProjectDataRepository.GetCommonProjectAndRevisionData(projectPk);

        commonProjectAndRevisionData.Cases = await getProjectDataRepository.GetCases(projectPk);
        commonProjectAndRevisionData.Wells = await getProjectDataRepository.GetWells(projectPk);
        commonProjectAndRevisionData.Surfs = await getProjectDataRepository.GetSurfs(projectPk);
        commonProjectAndRevisionData.Substructures = await getProjectDataRepository.GetSubstructures(projectPk);
        commonProjectAndRevisionData.Topsides = await getProjectDataRepository.GetTopsides(projectPk);
        commonProjectAndRevisionData.Transports = await getProjectDataRepository.GetTransports(projectPk);
        commonProjectAndRevisionData.DrainageStrategies = await getProjectDataRepository.GetDrainageStrategies(projectPk);

        commonProjectAndRevisionData.ModifyTime = commonProjectAndRevisionData.Cases.Select(c => c.ModifyTime).Append(commonProjectAndRevisionData.ModifyTime).Max();

        return commonProjectAndRevisionData;
    }
}
