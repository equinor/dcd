using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles;

public class CessationOnshoreFacilitiesCostProfileController(CessationOnshoreFacilitiesCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile")]
    public async Task<CessationOnshoreFacilitiesCostProfileDto> CreateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateCessationOnshoreFacilitiesCostProfileDto dto)
    {
        return await service.CreateCessationOnshoreFacilitiesCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile/{costProfileId:guid}")]
    public async Task<CessationOnshoreFacilitiesCostProfileDto> UpdateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCessationOnshoreFacilitiesCostProfileDto dto)
    {
        return await service.UpdateCessationOnshoreFacilitiesCostProfile(projectId, caseId, costProfileId, dto);
    }
}
