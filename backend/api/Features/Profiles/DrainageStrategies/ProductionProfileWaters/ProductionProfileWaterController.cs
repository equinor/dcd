using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaters.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileWaters;

public class ProductionProfileWaterController(ProductionProfileWaterService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water")]
    public async Task<ProductionProfileWaterDto> CreateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileWaterDto dto)
    {
        return await service.CreateProductionProfileWater(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water/{profileId:guid}")]
    public async Task<ProductionProfileWaterDto> UpdateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileWaterDto dto)
    {
        return await service.UpdateProductionProfileWater(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
