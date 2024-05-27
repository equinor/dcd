using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/surfs")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class SurfsController : ControllerBase
{
    private readonly ISurfService _surfService;

    public SurfsController(
        ISurfService surfService
    )
    {
        _surfService = surfService;
    }

    [HttpPut("{surfId}")]
    public async Task<SurfDto> UpdateSurf(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromBody] APIUpdateSurfDto dto)
    {
        return await _surfService.UpdateSurf(caseId, surfId, dto);
    }

    [HttpPut("{surfId}/cost-profile-override/{costProfileId}")]
    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid surfId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSurfCostProfileOverrideDto dto)
    {
        return await _surfService.UpdateSurfCostProfileOverride(caseId, surfId, costProfileId, dto);
    }
}
