using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileGases;

public class ProductionProfileGasController(
    CreateTimeSeriesProfileWithConversionService createTimeSeriesProfileWithConversionService,
    UpdateTimeSeriesProfileWithConversionService updateTimeSeriesProfileWithConversionService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-gas")]
    public async Task<TimeSeriesCostDto> CreateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileWithConversionService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.ProductionProfileGas, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-gas/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateProductionProfileGas(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileWithConversionService.UpdateTimeSeriesProfile(projectId, caseId, profileId, ProfileTypes.ProductionProfileGas, dto);
    }
}
