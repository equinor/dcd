using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Surfs.SurfCostProfileOverrides;

public class SurfCostProfileOverrideController(SurfTimeSeriesService surfTimeSeriesService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/surfs/{surfId:guid}/cost-profile-override")]
    public async Task<TimeSeriesCostOverrideDto> CreateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] CreateTimeSeriesCostOverrideDto dto)
    {
        return await surfTimeSeriesService.CreateSurfCostProfileOverride(projectId, caseId, surfId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/surfs/{surfId:guid}/cost-profile-override/{costProfileId:guid}")]
    public async Task<TimeSeriesCostOverrideDto> UpdateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTimeSeriesCostOverrideDto dto)
    {
        return await surfTimeSeriesService.UpdateSurfCostProfileOverride(projectId, caseId, surfId, costProfileId, dto);
    }
}
