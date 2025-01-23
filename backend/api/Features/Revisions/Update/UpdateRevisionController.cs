using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Revisions.Update;

public class UpdateRevisionController(GetProjectDataService getProjectDataService, UpdateRevisionService updateRevisionService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<RevisionDataDto> UpdateRevision([FromRoute] Guid projectId, [FromRoute] Guid revisionId, [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        await updateRevisionService.UpdateRevision(revisionId, updateRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
