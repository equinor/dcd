using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles;

public class CessationOnshoreFacilitiesCostProfileController(CessationOnshoreFacilitiesCostProfileService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile")]
    public async Task<TimeSeriesCostDto> CreateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostDto dto)
    {
        return await service.CreateCessationOnshoreFacilitiesCostProfile(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-onshore-facilities-cost-profile/{costProfileId:guid}")]
    public async Task<TimeSeriesCostDto> UpdateCessationOnshoreFacilitiesCostProfile(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostDto dto)
    {
        return await service.UpdateCessationOnshoreFacilitiesCostProfile(projectId, caseId, costProfileId, dto);
    }
}
