using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideController(UpdateTopsideService updateTopsideService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}")]
    public async Task<TopsideDto> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] UpdateTopsideDto dto)
    {
        return await updateTopsideService.UpdateTopside(projectId, caseId, topsideId, dto);
    }
}
