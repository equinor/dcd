using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides;

public class GasProducerCostProfileOverrideController(GasProducerCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-producer-cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await service.CreateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-producer-cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await service.UpdateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }
}
