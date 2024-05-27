using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/cases/{caseId}/explorations")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class ExplorationsController : ControllerBase
{
    private readonly IExplorationService _explorationService;

    public ExplorationsController(
        IExplorationService explorationService
    )
    {
        _explorationService = explorationService;
    }

    [HttpPut("{explorationId}")]
    public async Task<ExplorationDto> UpdateExploration(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] UpdateExplorationDto dto)
    {
        return await _explorationService.UpdateExploration(caseId, explorationId, dto);
    }

    [HttpPut("{explorationId}/seismic-acquisition-and-processing/{costProfileId}")]
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateSeismicAcquisitionAndProcessingDto dto)
    {
        return await _explorationService.UpdateSeismicAcquisitionAndProcessing(caseId, explorationId, costProfileId, dto);
    }

    [HttpPut("{explorationId}/country-office-cost/{costProfileId}")]
    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
    [FromRoute] Guid projectId,
    [FromRoute] Guid caseId,
    [FromRoute] Guid explorationId,
    [FromRoute] Guid costProfileId,
    [FromBody] UpdateCountryOfficeCostDto dto)
    {
        return await _explorationService.UpdateCountryOfficeCost(caseId, explorationId, costProfileId, dto);
    }
}
