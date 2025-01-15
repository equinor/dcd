using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles;

public class OnshoreRelatedOpexCostProfileController(OnshoreRelatedOpexCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/onshore-related-opex-cost-profile")]
    public async Task<OnshoreRelatedOpexCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateOnshoreRelatedOpexCostProfileDto dto)
    {
        return await service.CreateOnshoreRelatedOpexCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-related-opex-cost-profile/{costProfileId:guid}")]
    public async Task<OnshoreRelatedOpexCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOnshoreRelatedOpexCostProfileDto dto)
    {
        return await service.UpdateOnshoreRelatedOpexCostProfile(projectId, caseId, costProfileId, dto);
    }
}
