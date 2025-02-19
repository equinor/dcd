using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Save;

public class SaveProfileController(SaveProfileService saveProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/profiles/save")]
    public async Task<TimeSeriesDto> SaveProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] SaveTimeSeriesDto dto)
    {
        SaveProfileDtoValidator.Validate(dto);

        return await saveProfileService.SaveTimeSeriesProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/override-profiles/save")]
    public async Task<TimeSeriesOverrideDto> SaveOverrideProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] SaveTimeSeriesOverrideDto dto)
    {
        SaveProfileDtoValidator.Validate(dto);

        return await saveProfileService.SaveTimeSeriesOverrideProfile(projectId, caseId, dto);
    }
}
