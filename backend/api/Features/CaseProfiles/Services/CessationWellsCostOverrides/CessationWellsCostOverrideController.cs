using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.CessationWellsCostOverrides;

public class CessationWellsCostOverrideController(CessationWellsCostOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-wells-cost-override")]
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationWellsCostOverrideDto dto)
    {
        return await service.CreateCessationWellsCostOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-wells-cost-override/{costProfileId:guid}")]
    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationWellsCostOverrideDto dto)
    {
        return await service.UpdateCessationWellsCostOverride(projectId, caseId, costProfileId, dto);
    }
}
