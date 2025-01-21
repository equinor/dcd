using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.HistoricCostCostProfiles;

public class HistoricCostCostProfileController(HistoricCostCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/historic-cost-cost-profile")]
    public async Task<TimeSeriesCostDto> CreateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await service.CreateHistoricCostCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/historic-cost-cost-profile/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await service.UpdateHistoricCostCostProfile(projectId, caseId, costProfileId, dto);
    }
}
