using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases;

public class AdditionalProductionProfileGasController(
    CreateTimeSeriesProfileWithConversionService createTimeSeriesProfileWithConversionService,
    UpdateTimeSeriesProfileWithConversionService updateTimeSeriesProfileWithConversionService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-gas")]
    public async Task<TimeSeriesCostDto> CreateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileWithConversionService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.AdditionalProductionProfileGas, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/additional-production-profile-gas/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateAdditionalProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileWithConversionService.UpdateTimeSeriesProfile(projectId, caseId, profileId, ProfileTypes.AdditionalProductionProfileGas, dto);
    }
}
