using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.Cases.GetWithAssets;
using api.Features.Stea.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Topsides;

[Route("projects/{projectId}/cases/{caseId}/topsides")]
[AuthorizeActionType(ActionType.Edit)]
public class TopsidesController(
    TopsideService topsideService,
    TopsideTimeSeriesService topsideTimeSeriesService) : ControllerBase
{
    [HttpPut("{topsideId}")]
    public async Task<TopsideDto> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] APIUpdateTopsideDto dto)
    {
        return await topsideService.UpdateTopside(projectId, caseId, topsideId, dto);
    }

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
