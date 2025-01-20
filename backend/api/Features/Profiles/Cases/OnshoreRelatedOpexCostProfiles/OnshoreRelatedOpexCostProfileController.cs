using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles;

public class OnshoreRelatedOpexCostProfileController(OnshoreRelatedOpexCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/onshore-related-opex-cost-profile")]
    public async Task<TimeSeriesCostDto> CreateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await service.CreateOnshoreRelatedOpexCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/onshore-related-opex-cost-profile/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateOnshoreRelatedOPEXCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await service.UpdateOnshoreRelatedOpexCostProfile(projectId, caseId, costProfileId, dto);
    }
}
