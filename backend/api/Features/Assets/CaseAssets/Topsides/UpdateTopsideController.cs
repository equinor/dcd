using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideController(UpdateTopsideService updateTopsideService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/topside")]
    public async Task<NoContentResult> UpdateTopside(Guid projectId, Guid caseId, [FromBody] UpdateTopsideDto dto)
    {
        await updateTopsideService.UpdateTopside(projectId, caseId, dto);
        return NoContent();
    }
}
