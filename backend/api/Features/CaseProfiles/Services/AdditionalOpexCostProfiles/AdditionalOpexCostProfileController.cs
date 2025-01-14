using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles;

public class AdditionalOpexCostProfileController(AdditionalOpexCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/additional-opex-cost-profile")]
    public async Task<AdditionalOpexCostProfileDto> CreateAdditionalOpexCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateAdditionalOpexCostProfileDto dto)
    {
        return await service.CreateAdditionalOpexCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/additional-opex-cost-profile/{costProfileId:guid}")]
    public async Task<AdditionalOpexCostProfileDto> UpdateAdditionalOpexCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateAdditionalOpexCostProfileDto dto)
    {
        return await service.UpdateAdditionalOpexCostProfile(projectId, caseId, costProfileId, dto);
    }
}
