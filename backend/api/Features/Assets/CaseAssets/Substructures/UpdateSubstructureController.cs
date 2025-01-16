using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureController(UpdateSubstructureService updateSubstructureService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/substructures/{substructureId:guid}")]
    public async Task<NoContentResult> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] UpdateSubstructureDto dto)
    {
        await updateSubstructureService.UpdateSubstructure(projectId, caseId, substructureId, dto);
        return NoContent();
    }
}
