using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.WellProjects.OilProducerCostProfileOverrides;

public class OilProducerCostProfileOverrideController(OilProducerCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/oil-producer-cost-profile-override")]
    public async Task<OilProducerCostProfileOverrideDto> CreateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateOilProducerCostProfileOverrideDto dto)
    {
        return await service.CreateOilProducerCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/oil-producer-cost-profile-override/{costProfileId:guid}")]
    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOilProducerCostProfileOverrideDto dto)
    {
        return await service.UpdateOilProducerCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }
}
