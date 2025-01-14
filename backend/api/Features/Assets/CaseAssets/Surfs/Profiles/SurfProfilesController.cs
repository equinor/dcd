using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Surfs.Profiles;

[Route("projects/{projectId}/cases/{caseId}/surfs")]
[AuthorizeActionType(ActionType.Edit)]
public class SurfProfilesController(SurfTimeSeriesService surfTimeSeriesService) : ControllerBase
{
    [HttpPost("{surfId}/cost-profile-override/")]
    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] CreateSurfCostProfileOverrideDto dto)
    {
        return await surfTimeSeriesService.CreateSurfCostProfileOverride(projectId, caseId, surfId, dto);
    }

    [HttpPut("{surfId}/cost-profile-override/{costProfileId}")]
    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSurfCostProfileOverrideDto dto)
    {
        return await surfTimeSeriesService.UpdateSurfCostProfileOverride(projectId, caseId, surfId, costProfileId, dto);
    }
}
