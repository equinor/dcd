using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides;

public class Co2EmissionsOverrideController(Co2EmissionsOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/co2-emissions-override/")]
    public async Task<Co2EmissionsOverrideDto> CreateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateCo2EmissionsOverrideDto dto)
    {
        return await service.CreateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/co2-emissions-override/{profileId:guid}")]
    public async Task<Co2EmissionsOverrideDto> UpdateCo2EmissionsOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateCo2EmissionsOverrideDto dto)
    {
        return await service.UpdateCo2EmissionsOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
