using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileWaters;

public class ProductionProfileWaterController(
    CreateTimeSeriesProfileWithConversionService createTimeSeriesProfileWithConversionService,
    UpdateTimeSeriesProfileWithConversionService updateTimeSeriesProfileWithConversionService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water")]
    public async Task<TimeSeriesCostDto> CreateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileWithConversionService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.ProductionProfileWater, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-water/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateProductionProfileWater(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileWithConversionService.UpdateTimeSeriesProfile(projectId, caseId, profileId, ProfileTypes.ProductionProfileWater, dto);
    }
}
