using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileGases;

public class ProductionProfileGasController(ProductionProfileGasService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-gas/")]
    public async Task<ProductionProfileGasDto> CreateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateProductionProfileGasDto dto)
    {
        return await service.CreateProductionProfileGas(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-gas/{profileId:guid}")]
    public async Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateProductionProfileGasDto dto)
    {
        return await service.UpdateProductionProfileGas(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
