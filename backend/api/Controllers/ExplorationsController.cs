using api.Adapters;
using api.Dtos;
using api.Models;
using api.Services;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User

    )]
public class ExplorationsController : ControllerBase
{
    private readonly IExplorationService _explorationService;

    public ExplorationsController(IExplorationService explorationService)
    {
        _explorationService = explorationService;
    }

    [HttpPost(Name = "CreateExploration")]
    public async Task<ProjectDto> CreateExploration([FromQuery] Guid sourceCaseId, [FromBody] ExplorationDto explorationDto)
    {
        return await _explorationService.CreateExploration(explorationDto, sourceCaseId);
    }

    [HttpDelete("{explorationId}", Name = "DeleteExploration")]
    public async Task<ProjectDto> DeleteExploration(Guid explorationId)
    {
        return await _explorationService.DeleteExploration(explorationId);
    }

    [HttpPut(Name = "UpdateExploration")]
    public async Task<ProjectDto> UpdateExploration([FromBody] ExplorationDto eplorationDto)
    {
        return await _explorationService.UpdateExploration(eplorationDto);
    }

    [HttpPut("new", Name = "NewUpdateExploration")]
    public async Task<ExplorationDto> NewUpdateExploration([FromBody] ExplorationDto eplorationDto)
    {
        return await _explorationService.NewUpdateExploration(eplorationDto);
    }

    [HttpPost("{explorationId}/copy", Name = "CopyExploration")]
    public async Task<ExplorationDto> CopyExploration([FromQuery] Guid caseId, Guid explorationId)
    {
        return await _explorationService.CopyExploration(explorationId, caseId);
    }
}
