using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases;

public class AdditionalProductionProfileGasController(AdditionalProductionProfileGasService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-gas")]
    public async Task<TimeSeriesVolumeDto> CreateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesVolumeDto dto)
    {
        return await service.CreateAdditionalProductionProfileGas(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-gas/{profileId:guid}")]
    public async Task<TimeSeriesVolumeDto> UpdateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesVolumeDto dto)
    {
        return await service.UpdateAdditionalProductionProfileGas(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
