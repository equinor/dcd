using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureController(UpdateSubstructureService updateSubstructureService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/substructure")]
    public async Task<NoContentResult> UpdateSubstructure(Guid projectId, Guid caseId, [FromBody] UpdateSubstructureDto dto)
    {
        await updateSubstructureService.UpdateSubstructure(projectId, caseId, dto);
        return NoContent();
    }
}
