using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides;

public class ImportedElectricityOverrideController(ImportedElectricityOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/imported-electricity-override")]
    public async Task<TimeSeriesEnergyOverrideDto> CreateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromBody] CreateTimeSeriesEnergyDto dto)
    {
        return await service.CreateImportedElectricityOverride(projectId, caseId, drainageStrategyId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/drainage-strategies/{drainageStrategyId:guid}/imported-electricity-override/{profileId:guid}")]
    public async Task<TimeSeriesEnergyOverrideDto> UpdateImportedElectricityOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid drainageStrategyId,
        [FromRoute] Guid profileId,
        [FromBody] UpdateTimeSeriesEnergyOverrideDto dto)
    {
        return await service.UpdateImportedElectricityOverride(projectId, caseId, drainageStrategyId, profileId, dto);
    }
}
