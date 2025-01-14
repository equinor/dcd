using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Profiles.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Topsides.Profiles;

[Route("projects/{projectId}/cases/{caseId}/topsides")]
[AuthorizeActionType(ActionType.Edit)]
public class TopsideProfilesController(TopsideTimeSeriesService topsideTimeSeriesService) : ControllerBase
{
    [HttpPost("{topsideId}/cost-profile-override/")]
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] CreateTopsideCostProfileOverrideDto dto)
    {
        return await topsideTimeSeriesService.CreateTopsideCostProfileOverride(projectId, caseId, topsideId, dto);
    }

    [HttpPut("{topsideId}/cost-profile-override/{costProfileId}")]
    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTopsideCostProfileOverrideDto dto)
    {
        return await topsideTimeSeriesService.UpdateTopsideCostProfileOverride(projectId, caseId, topsideId, costProfileId, dto);
    }
}
