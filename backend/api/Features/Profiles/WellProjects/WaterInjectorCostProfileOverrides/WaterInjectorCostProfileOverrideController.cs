using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides.Dtos;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.WellProjects.WaterInjectorCostProfileOverrides;

public class WaterInjectorCostProfileOverrideController(WaterInjectorCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/water-injector-cost-profile-override")]
    public async Task<WaterInjectorCostProfileOverrideDto> CreateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateWaterInjectorCostProfileOverrideDto dto)
    {
        return await service.CreateWaterInjectorCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut($"projects/{{projectId:guid}}/cases/{{caseId:guid}}/well-projects/{{wellProjectId:guid}}/water-injector-cost-profile-override/{{{nameof(costProfileId)}}}")]
    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateWaterInjectorCostProfileOverrideDto dto)
    {
        return await service.UpdateWaterInjectorCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }
}
