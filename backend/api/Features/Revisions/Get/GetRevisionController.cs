using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Revisions.Get;

public class GetRevisionController(GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [AuthorizeActionType(ActionType.Read)]
    public async Task<RevisionDataDto> Get(Guid projectId, Guid revisionId)
    {
        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
