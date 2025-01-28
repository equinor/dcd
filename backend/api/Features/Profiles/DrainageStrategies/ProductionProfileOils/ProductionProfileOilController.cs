using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Create;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.Update;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ProductionProfileOils;

public class ProductionProfileOilController(
    CreateTimeSeriesProfileWithConversionService createTimeSeriesProfileWithConversionService,
    UpdateTimeSeriesProfileWithConversionService updateTimeSeriesProfileWithConversionService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-oil")]
    public async Task<TimeSeriesCostDto> CreateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await createTimeSeriesProfileWithConversionService.CreateTimeSeriesProfile(projectId, caseId, ProfileTypes.ProductionProfileOil, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/production-profile-oil/{profileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateProductionProfileOil(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await updateTimeSeriesProfileWithConversionService.UpdateTimeSeriesProfile(projectId, caseId, profileId, ProfileTypes.ProductionProfileOil, dto);
    }
}
