using api.AppInfrastructure.ControllerAttributes;
using api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides;

public class OffshoreFacilitiesOperationsCostProfileOverrideController(OffshoreFacilitiesOperationsCostProfileOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/offshore-facilities-operations-cost-profile-override")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOffshoreFacilitiesOperationsCostProfileOverrideDto dto)
    {
        return await service.CreateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/offshore-facilities-operations-cost-profile-override/{costProfileId:guid}")]
    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto dto)
    {
        return await service.UpdateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, costProfileId, dto);
    }
}
