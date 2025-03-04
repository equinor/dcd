using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Revisions.Delete;

public class DeleteRevisionController(DeleteRevisionService deleteRevisionService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<NoContentResult> DeleteRevision(Guid projectId, Guid revisionId)
    {
        await deleteRevisionService.DeleteRevision(projectId, revisionId);

        return new NoContentResult();
    }
}
