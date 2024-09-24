using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/surfs")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class SurfsController : ControllerBase
{
    private readonly ISurfService _surfService;
    private readonly ISurfTimeSeriesService _surfTimeSeriesService;

    public SurfsController(
        ISurfService surfService,
        ISurfTimeSeriesService surfTimeSeriesService
    )
    {
        _surfService = surfService;
        _surfTimeSeriesService = surfTimeSeriesService;
    }

    [HttpPut("{surfId}")]
    public async Task<SurfDto> UpdateSurf(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] APIUpdateSurfDto dto)
    {
        return await _surfService.UpdateSurf(projectId, caseId, surfId, dto);
    }

    [HttpPost("{surfId}/cost-profile-override/")]
    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] CreateSurfCostProfileOverrideDto dto)
    {
        return await _surfTimeSeriesService.CreateSurfCostProfileOverride(projectId, caseId, surfId, dto);
    }

    [HttpPut("{surfId}/cost-profile-override/{costProfileId}")]
    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSurfCostProfileOverrideDto dto)
    {
        return await _surfTimeSeriesService.UpdateSurfCostProfileOverride(projectId, caseId, surfId, costProfileId, dto);
    }
}
