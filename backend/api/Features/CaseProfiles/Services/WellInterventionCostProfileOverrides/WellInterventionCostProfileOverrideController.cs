using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides;

public class WellInterventionCostProfileOverrideController(WellInterventionCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-intervention-cost-profile-override")]
    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateWellInterventionCostProfileOverrideDto dto)
    {
        return await service.CreateWellInterventionCostProfileOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-intervention-cost-profile-override/{costProfileId:guid}")]
    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWellInterventionCostProfileOverrideDto dto)
    {
        return await service.UpdateWellInterventionCostProfileOverride(projectId, caseId, costProfileId, dto);
    }
}
