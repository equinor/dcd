using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/topsides")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class TopsidesController : ControllerBase
{
    private readonly ITopsideService _topsideService;

    public TopsidesController(
        ITopsideService topsideService
    )
    {
        _topsideService = topsideService;
    }

    [HttpPut("{topsideId}")]
    public async Task<TopsideDto> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] APIUpdateSubstructureDto dto)
    {
        return await _topsideService.UpdateTopside(caseId, topsideId, dto);
    }

    [HttpPut("{topsideId}/cost-profile-override/{costProfileId}")]
    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTopsideCostProfileOverrideDto dto)
    {
        return await _topsideService.UpdateSubstructureCostProfileOverride(caseId, topsideId, costProfileId, dto);
    }


}
