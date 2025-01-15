using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides;

public class FuelFlaringAndLossesOverrideController(FuelFlaringAndLossesOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/fuel-flaring-and-losses-override")]
    public async Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateFuelFlaringAndLossesOverrideDto dto)
    {
        return await service.CreateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/fuel-flaring-and-losses-override/{profileId:guid}")]
    public async Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateFuelFlaringAndLossesOverrideDto dto)
    {
        return await service.UpdateFuelFlaringAndLossesOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
