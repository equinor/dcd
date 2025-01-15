using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Explorations.GAndGAdminCostOverrides;

public class GAndGAdminCostOverrideController(GAndGAdminCostOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/g-and-g-and-admin-cost-override")]
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateGAndGAdminCostOverrideDto dto)
    {
        return await service.CreateGAndGAdminCostOverride(projectId, caseId, explorationId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/g-and-g-and-admin-cost-override/{costProfileId:guid}")]
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGAndGAdminCostOverrideDto dto)
    {
        return await service.UpdateGAndGAdminCostOverride(projectId, caseId, explorationId, costProfileId, dto);
    }
}
