using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides;

public class OffshoreFacilitiesOperationsCostProfileOverrideController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/offshore-facilities-operations-cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/offshore-facilities-operations-cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, costProfileId, ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride, dto);
    }
}
