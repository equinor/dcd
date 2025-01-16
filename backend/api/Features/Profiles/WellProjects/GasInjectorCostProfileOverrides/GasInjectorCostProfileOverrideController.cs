using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.WellProjects.GasInjectorCostProfileOverrides;

public class GasInjectorCostProfileOverrideController(GasInjectorCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-injector-cost-profile-override")]
    public async Task<GasInjectorCostProfileOverrideDto> CreateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasInjectorCostProfileOverrideDto dto)
    {
        return await service.CreateGasInjectorCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-injector-cost-profile-override/{costProfileId:guid}")]
    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasInjectorCostProfileOverrideDto dto)
    {
        return await service.UpdateGasInjectorCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }
}
