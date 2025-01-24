using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides;

public class TotalFeasibilityAndConceptStudiesOverrideController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/total-feasibility-and-concept-studies-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesOverrideProfile(projectId, caseId, ProfileTypes.TotalFeasibilityAndConceptStudiesOverride, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/total-feasibility-and-concept-studies-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesOverrideProfile(projectId, caseId, costProfileId, ProfileTypes.TotalFeasibilityAndConceptStudiesOverride, dto);
    }
}
