using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.AdditionalOpexCostProfiles;

public class AdditionalOpexCostProfileController(AdditionalOpexCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/additional-opex-cost-profile")]
    public async Task<TimeSeriesCostDto> CreateAdditionalOpexCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await service.CreateAdditionalOpexCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/additional-opex-cost-profile/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateAdditionalOpexCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await service.UpdateAdditionalOpexCostProfile(projectId, caseId, costProfileId, dto);
    }
}
