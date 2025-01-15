using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides.Dtos;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.WellProjects.GasProducerCostProfileOverrides;

public class GasProducerCostProfileOverrideController(GasProducerCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-producer-cost-profile-override")]
    public async Task<GasProducerCostProfileOverrideDto> CreateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromBody] CreateGasProducerCostProfileOverrideDto dto)
    {
        return await service.CreateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/well-projects/{wellProjectId:guid}/gas-producer-cost-profile-override/{costProfileId:guid}")]
    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid wellProjectId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateGasProducerCostProfileOverrideDto dto)
    {
        return await service.UpdateGasProducerCostProfileOverride(projectId, caseId, wellProjectId, costProfileId, dto);
    }
}
