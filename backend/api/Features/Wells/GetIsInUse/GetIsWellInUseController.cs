using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Wells.GetIsInUse;

public class GetIsWellInUseController(GetIsWellInUseService getIsWellInUseService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/wells/{wellId:guid}/is-in-use")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<bool> IsWellInUse([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        return await getIsWellInUseService.IsWellInUse(projectId, wellId);
    }
}
