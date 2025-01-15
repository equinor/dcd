using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles;

public class TotalOtherStudiesCostProfileController(TotalOtherStudiesCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/total-other-studies-cost-profile")]
    public async Task<TotalOtherStudiesCostProfileDto> CreateTotalOtherStudiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTotalOtherStudiesCostProfileDto dto)
    {
        return await service.CreateTotalOtherStudiesCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/total-other-studies-cost-profile/{costProfileId:guid}")]
    public async Task<TotalOtherStudiesCostProfileDto> UpdateTotalOtherStudiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTotalOtherStudiesCostProfileDto dto)
    {
        return await service.UpdateTotalOtherStudiesCostProfile(projectId, caseId, costProfileId, dto);
    }
}
