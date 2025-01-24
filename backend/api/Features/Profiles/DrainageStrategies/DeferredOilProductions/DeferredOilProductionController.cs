using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.DeferredOilProductions;

public class DeferredOilProductionController(DeferredOilProductionService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/deferred-oil-production/")]
    public async Task<TimeSeriesVolumeDto> CreateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesVolumeDto dto)
    {
        return await service.CreateDeferredOilProduction(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/deferred-oil-production/{profileId:guid}")]
    public async Task<TimeSeriesVolumeDto> UpdateDeferredOilProduction(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesVolumeDto dto)
    {
        return await service.UpdateDeferredOilProduction(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
