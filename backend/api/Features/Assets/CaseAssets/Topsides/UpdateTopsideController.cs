using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideController(UpdateTopsideService updateTopsideService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}")]
    public async Task<NoContentResult> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] UpdateTopsideDto dto)
    {
        await updateTopsideService.UpdateTopside(projectId, caseId, dto);
        return NoContent();
    }
}
