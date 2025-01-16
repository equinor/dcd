using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Substructures.SubstructureCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Substructures.SubstructureCostProfileOverrides;

public class SubstructureCostProfileOverrideController(SubstructureCostProfileOverrideService substructureCostProfileOverrideService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/substructures/{substructureId:guid}/cost-profile-override")]
    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromBody] CreateSubstructureCostProfileOverrideDto dto)
    {
        return await substructureCostProfileOverrideService.CreateSubstructureCostProfileOverride(projectId, caseId, substructureId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/substructures/{substructureId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid substructureId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSubstructureCostProfileOverrideDto dto)
    {
        return await substructureCostProfileOverrideService.UpdateSubstructureCostProfileOverride(projectId, caseId, substructureId, costProfileId, dto);
    }
}
