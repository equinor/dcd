using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileOils;

public class ProductionProfileOilController(ProductionProfileOilService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-oil")]
    public async Task<TimeSeriesVolumeDto> CreateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesVolumeDto dto)
    {
        return await service.CreateProductionProfileOil(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-oil/{profileId:guid}")]
    public async Task<TimeSeriesVolumeDto> UpdateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesVolumeDto dto)
    {
        return await service.UpdateProductionProfileOil(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
