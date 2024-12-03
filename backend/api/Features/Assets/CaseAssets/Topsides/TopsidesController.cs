using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Assets.CaseAssets.Topsides;

[ApiController]
[Route("projects/{projectId}/cases/{caseId}/topsides")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.User
)]
[ActionType(ActionType.Edit)]
public class TopsidesController(
    ITopsideService topsideService,
    ITopsideTimeSeriesService topsideTimeSeriesService) : ControllerBase
{
    [HttpPut("{topsideId}")]
    public async Task<TopsideDto> UpdateTopside(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] APIUpdateTopsideDto dto)
    {
        return await topsideService.UpdateTopside(projectId, caseId, topsideId, dto);
    }

    [HttpPost("{topsideId}/cost-profile-override/")]
    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromBody] CreateTopsideCostProfileOverrideDto dto)
    {
        return await topsideTimeSeriesService.CreateTopsideCostProfileOverride(projectId, caseId, topsideId, dto);
    }

    [HttpPut("{topsideId}/cost-profile-override/{costProfileId}")]
    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid topsideId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateTopsideCostProfileOverrideDto dto)
    {
        return await topsideTimeSeriesService.UpdateTopsideCostProfileOverride(projectId, caseId, topsideId, costProfileId, dto);
    }
}
