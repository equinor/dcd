using api.Features.ProjectData.Dtos;

namespace api.Features.ProjectData;

public class GetProjectDataService(GetProjectDataRepository getProjectDataRepository)
{
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        var projectMembers = await getProjectDataRepository.GetProjectMembers(projectId);
        var revisionDetailsList = await getProjectDataRepository.GetRevisionDetailsList(projectId);

        var commonProjectAndRevisionData = await getProjectDataRepository.GetCommonProjectAndRevisionData(projectId);
        commonProjectAndRevisionData.Cases = await getProjectDataRepository.GetCases(projectId);
        commonProjectAndRevisionData.Wells = await getProjectDataRepository.GetWells(projectId);
        commonProjectAndRevisionData.Surfs = await getProjectDataRepository.GetSurfs(projectId);
        commonProjectAndRevisionData.Substructures = await getProjectDataRepository.GetSubstructures(projectId);
        commonProjectAndRevisionData.Topsides = await getProjectDataRepository.GetTopsides(projectId);
        commonProjectAndRevisionData.Transports = await getProjectDataRepository.GetTransports(projectId);
        commonProjectAndRevisionData.DrainageStrategies = await getProjectDataRepository.GetDrainageStrategies(projectId);

        return new ProjectDataDto
        {
            ProjectId = projectId,
            ProjectMembers = projectMembers,
            RevisionDetailsList = revisionDetailsList.OrderBy(x => x.RevisionDate).ToList(),
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }

    public async Task<RevisionDataDto> GetRevisionData(Guid projectId, Guid revisionId)
    {
        var projectPk = await getProjectDataRepository.GetProjectIdForRevision(projectId, revisionId);

        var revisionDetails = await getProjectDataRepository.GetRevisionDetails(projectPk, revisionId);

        var commonProjectAndRevisionData = await getProjectDataRepository.GetCommonProjectAndRevisionData(projectPk);
        commonProjectAndRevisionData.Cases = await getProjectDataRepository.GetCases(projectPk);
        commonProjectAndRevisionData.Wells = await getProjectDataRepository.GetWells(projectPk);
        commonProjectAndRevisionData.Surfs = await getProjectDataRepository.GetSurfs(projectId);
        commonProjectAndRevisionData.Substructures = await getProjectDataRepository.GetSubstructures(projectPk);
        commonProjectAndRevisionData.Topsides = await getProjectDataRepository.GetTopsides(projectPk);
        commonProjectAndRevisionData.Transports = await getProjectDataRepository.GetTransports(projectPk);
        commonProjectAndRevisionData.DrainageStrategies = await getProjectDataRepository.GetDrainageStrategies(projectPk);

        return new RevisionDataDto
        {
            ProjectId = projectId,
            RevisionId = revisionId,
            RevisionDetails = revisionDetails,
            CommonProjectAndRevisionData = commonProjectAndRevisionData
        };
    }
}
