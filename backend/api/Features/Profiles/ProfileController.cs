using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles;

public class ProfileController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/profiles")]
    public async Task<TimeSeriesCostDto> CreateProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesDto dto)
    {
        TimeSeriesDtoValidator.Validate(dto);

        return await createTimeSeriesProfileService.CreateTimeSeriesProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/profiles/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesDto dto)
    {
        TimeSeriesDtoValidator.Validate(dto);

        return await updateTimeSeriesProfileService.UpdateTimeSeriesProfile(projectId, caseId, profileId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/override-profiles")]
    public async Task<TimeSeriesCostOverrideDto> CreateOverrideProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesOverrideDto dto)
    {
        TimeSeriesDtoValidator.Validate(dto);

        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/override-profiles/{profileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateOverrideProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesOverrideDto dto)
    {
        TimeSeriesDtoValidator.Validate(dto);

        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, profileId, dto);
    }
}
