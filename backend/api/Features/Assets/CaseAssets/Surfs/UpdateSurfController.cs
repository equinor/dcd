using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfController(UpdateSurfService updateSurfService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/surf")]
    public async Task<NoContentResult> UpdateSurf(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] UpdateSurfDto dto)
    {
        await updateSurfService.UpdateSurf(projectId, caseId, dto);
        return NoContent();
    }
}
