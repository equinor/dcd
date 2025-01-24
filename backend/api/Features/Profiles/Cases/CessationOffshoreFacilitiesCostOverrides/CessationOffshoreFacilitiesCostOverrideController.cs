using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides;

public class CessationOffshoreFacilitiesCostOverrideController(CessationOffshoreFacilitiesCostOverrideService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/cessation-offshore-facilities-cost-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await service.CreateCessationOffshoreFacilitiesCostOverride(projectId, caseId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/cessation-offshore-facilities-cost-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await service.UpdateCessationOffshoreFacilitiesCostOverride(projectId, caseId, costProfileId, dto);
    }
}
