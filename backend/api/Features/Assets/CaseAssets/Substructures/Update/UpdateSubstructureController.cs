using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Substructures.Update;

public class UpdateSubstructureController(SubstructureService substructureService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/substructures/{substructureId:guid}")]
    public async Task<SubstructureDto> UpdateSubstructure(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] UpdateSubstructureDto dto)
    {
        return await substructureService.UpdateSubstructure(projectId, caseId, substructureId, dto);
    }
}
