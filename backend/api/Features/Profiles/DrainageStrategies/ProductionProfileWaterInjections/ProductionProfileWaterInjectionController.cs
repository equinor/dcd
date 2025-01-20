using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections;

public class ProductionProfileWaterInjectionController(ProductionProfileWaterInjectionService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water-injection")]
    public async Task<TimeSeriesVolumeDto> CreateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesVolumeDto dto)
    {
        return await service.CreateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water-injection/{profileId:guid}")]
    public async Task<TimeSeriesVolumeDto> UpdateProductionProfileWaterInjection(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesVolumeDto dto)
    {
        return await service.UpdateProductionProfileWaterInjection(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
