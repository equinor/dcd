using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.TotalFeedStudiesOverrides;

public class TotalFeedStudiesOverrideController(TotalFeedStudiesOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/total-feed-studies-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateTotalFeedStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await service.CreateTotalFeedStudiesOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/total-feed-studies-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateTotalFeedStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await service.UpdateTotalFeedStudiesOverride(projectId, caseId, costProfileId, dto);
    }
}
