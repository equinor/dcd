using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Topsides.TopsideCostProfileOverrides;

public class TopsideCostProfileOverrideController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}/cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, ProfileTypes.TopsideCostProfileOverride, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/topsides/{topsideId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, costProfileId, ProfileTypes.TopsideCostProfileOverride, dto);
    }
}
