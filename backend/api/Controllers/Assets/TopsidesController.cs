using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/topsides")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class TopsidesController : ControllerBase
{
    private readonly ITopsideService _topsideService;
    private readonly ITopsideTimeSeriesService _topsideTimeSeriesService;

    public TopsidesController(
        ITopsideService topsideService,
        ITopsideTimeSeriesService topsideTimeSeriesService
    )
    {
        _topsideService = topsideService;
        _topsideTimeSeriesService = topsideTimeSeriesService;
    }

    [HttpPut("{topsideId}")]
    public async Task<TopsideDto> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] APIUpdateTopsideDto dto)
    {
        return await _topsideService.UpdateTopside(projectId, caseId, topsideId, dto);
    }

    [HttpPost("{topsideId}/cost-profile-override/")]
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] CreateTopsideCostProfileOverrideDto dto)
    {
        return await _topsideTimeSeriesService.CreateTopsideCostProfileOverride(projectId, caseId, topsideId, dto);
    }

    [HttpPut("{topsideId}/cost-profile-override/{costProfileId}")]
    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTopsideCostProfileOverrideDto dto)
    {
        return await _topsideTimeSeriesService.UpdateTopsideCostProfileOverride(projectId, caseId, topsideId, costProfileId, dto);
    }
}
