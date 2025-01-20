using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils;

public class AdditionalProductionProfileOilController(AdditionalProductionProfileOilService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-oil")]
    public async Task<TimeSeriesVolumeDto> CreateAdditionalProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesVolumeDto dto)
    {
        return await service.CreateAdditionalProductionProfileOil(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-oil/{profileId:guid}")]
    public async Task<TimeSeriesVolumeDto> UpdateAdditionalProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesVolumeDto dto)
    {
        return await service.UpdateAdditionalProductionProfileOil(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
