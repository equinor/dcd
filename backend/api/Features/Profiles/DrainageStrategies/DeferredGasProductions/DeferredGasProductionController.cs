using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.DeferredGasProductions;

public class DeferredGasProductionController(DeferredGasProductionService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/deferred-gas-production")]
    public async Task<DeferredGasProductionDto> CreateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateDeferredGasProductionDto dto)
    {
        return await service.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/deferred-gas-production/{profileId:guid}")]
    public async Task<DeferredGasProductionDto> UpdateDeferredGasProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateDeferredGasProductionDto dto)
    {
        return await service.UpdateDeferredGasProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
