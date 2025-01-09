using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Wells.Delete;

public class DeleteWellController(DeleteWellService deleteWellService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/wells/{wellId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task DeleteWell([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        await deleteWellService.DeleteWell(projectId, wellId);
    }
}
