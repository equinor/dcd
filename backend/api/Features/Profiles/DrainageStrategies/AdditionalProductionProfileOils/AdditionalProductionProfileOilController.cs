using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils;

public class AdditionalProductionProfileOilController(
    CreateTimeSeriesProfileService createTimeSeriesProfileService,
    UpdateTimeSeriesProfileService updateTimeSeriesProfileService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-oil")]
    public async Task<TimeSeriesCostDto> CreateAdditionalProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.AdditionalProductionProfileOil, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-oil/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateAdditionalProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileService.UpdateTimeSeriesProfile(projectId, caseId, profileId, ProfileTypes.AdditionalProductionProfileOil, dto);
    }
}
