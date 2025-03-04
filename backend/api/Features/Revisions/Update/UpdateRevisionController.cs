using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Revisions.Update;

public class UpdateRevisionController(GetProjectDataService getProjectDataService, UpdateRevisionService updateRevisionService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<RevisionDataDto> UpdateRevision(Guid projectId, Guid revisionId, [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        await updateRevisionService.UpdateRevision(projectId, revisionId, updateRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
