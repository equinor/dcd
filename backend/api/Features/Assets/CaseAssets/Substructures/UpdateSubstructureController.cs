using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureController(UpdateSubstructureService updateSubstructureService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/substructures/{substructureId:guid}")]
    public async Task<SubstructureDto> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] UpdateSubstructureDto dto)
    {
        return await updateSubstructureService.UpdateSubstructure(projectId, caseId, substructureId, dto);
    }
}
