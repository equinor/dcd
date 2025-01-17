using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Topsides.TopsideCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Topsides.TopsideCostProfileOverrides;

public class TopsideCostProfileOverrideController(TopsideCostProfileOverrideService topsideCostProfileOverrideService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}/cost-profile-override")]
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] CreateTopsideCostProfileOverrideDto dto)
    {
        return await topsideCostProfileOverrideService.CreateTopsideCostProfileOverride(projectId, caseId, topsideId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTopsideCostProfileOverrideDto dto)
    {
        return await topsideCostProfileOverrideService.UpdateTopsideCostProfileOverride(projectId, caseId, topsideId, costProfileId, dto);
    }
}
