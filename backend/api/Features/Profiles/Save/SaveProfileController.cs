using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Save;

public class SaveProfileController(SaveProfileService saveProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/profiles/save-batch")]
    public async Task<NoContentResult> SaveProfiles(Guid projectId, Guid caseId, [FromBody] SaveTimeSeriesListDto dto)
    {
        SaveProfileDtoValidator.Validate(dto);

        await saveProfileService.SaveTimeSeriesList(projectId, caseId, dto);

        return new NoContentResult();
    }
}
