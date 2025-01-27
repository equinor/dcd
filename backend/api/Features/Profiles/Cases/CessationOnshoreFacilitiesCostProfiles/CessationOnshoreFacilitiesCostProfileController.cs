using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles;

public class CessationOnshoreFacilitiesCostProfileController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile")]
    public async Task<TimeSeriesCostDto> CreateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.CessationOnshoreFacilitiesCostProfile, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesProfile(projectId, caseId, costProfileId, ProfileTypes.CessationOnshoreFacilitiesCostProfile, dto);
    }
}
