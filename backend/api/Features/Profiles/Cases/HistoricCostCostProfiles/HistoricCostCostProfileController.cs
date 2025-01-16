using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Cases.HistoricCostCostProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.HistoricCostCostProfiles;

public class HistoricCostCostProfileController(HistoricCostCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/historic-cost-cost-profile")]
    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateHistoricCostCostProfileDto dto)
    {
        return await service.CreateHistoricCostCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/historic-cost-cost-profile/{costProfileId:guid}")]
    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateHistoricCostCostProfileDto dto)
    {
        return await service.UpdateHistoricCostCostProfile(projectId, caseId, costProfileId, dto);
    }
}
