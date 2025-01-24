using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.CessationWellsCostOverrides;

public class CessationWellsCostOverrideController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-wells-cost-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, ProfileTypes.CessationWellsCostOverride, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-wells-cost-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, costProfileId, ProfileTypes.CessationWellsCostOverride, dto);
    }
}
