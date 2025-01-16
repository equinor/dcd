using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides;

public class TotalFeasibilityAndConceptStudiesOverrideController(TotalFeasibilityAndConceptStudiesOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/total-feasibility-and-concept-studies-override")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalFeasibilityAndConceptStudiesOverrideDto dto)
    {
        return await service.CreateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/total-feasibility-and-concept-studies-override/{costProfileId:guid}")]
    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalFeasibilityAndConceptStudiesOverrideDto dto)
    {
        return await service.UpdateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, costProfileId, dto);
    }
}
