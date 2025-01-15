using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides;

public class NetSalesGasOverrideController(NetSalesGasOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/net-sales-gas-override/")]
    public async Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateNetSalesGasOverrideDto dto)
    {
        return await service.CreateNetSalesGasOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/net-sales-gas-override/{profileId:guid}")]
    public async Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateNetSalesGasOverrideDto dto)
    {
        return await service.UpdateNetSalesGasOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
