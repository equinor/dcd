using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Surfs;

public class UpdateSurfController(UpdateSurfService updateSurfService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/surfs/{surfId:guid}")]
    public async Task<SurfDto> UpdateSurf(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] UpdateSurfDto dto)
    {
        return await updateSurfService.UpdateSurf(projectId, caseId, surfId, dto);
    }
}
