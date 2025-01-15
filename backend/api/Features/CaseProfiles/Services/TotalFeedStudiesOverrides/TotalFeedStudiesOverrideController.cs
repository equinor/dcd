using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.TotalFeedStudiesOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.TotalFeedStudiesOverrides;

public class TotalFeedStudiesOverrideController(TotalFeedStudiesOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/total-feed-studies-override")]
    public async Task<TotalFeedStudiesOverrideDto> CreateTotalFeedStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFeedStudiesOverrideDto dto)
    {
        return await service.CreateTotalFeedStudiesOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/total-feed-studies-override/{costProfileId:guid}")]
    public async Task<TotalFeedStudiesOverrideDto> UpdateTotalFeedStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFeedStudiesOverrideDto dto)
    {
        return await service.UpdateTotalFeedStudiesOverride(projectId, caseId, costProfileId, dto);
    }
}
