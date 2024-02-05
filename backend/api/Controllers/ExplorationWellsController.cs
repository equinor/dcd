
using api.Dtos;
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
public class ExplorationWellsController : ControllerBase
{

    private readonly IExplorationWellService _explorationWellService;

    public ExplorationWellsController(IExplorationWellService explorationWellService)
    {
        _explorationWellService = explorationWellService;
    }

    [HttpGet(Name = "GetExplorationWells")]
    public async Task<IEnumerable<ExplorationWellDto>> GetExplorationWells([FromQuery] Guid projectId)
    {
        return await _explorationWellService.GetAllDtos();
    }

    [HttpPost(Name = "CreateExplorationWell")]
    public async Task<ProjectDto> CreateExplorationWell([FromBody] ExplorationWellDto wellDto)
    {
        return await _explorationWellService.CreateExplorationWell(wellDto);
    }

    [HttpPut(Name = "UpdateExplorationWell")]
    public async Task<ProjectDto> UpdateExplorationWell([FromBody] ExplorationWellDto wellDto)
    {
        return await _explorationWellService.UpdateExplorationWell(wellDto);
    }

    [HttpPost("multiple", Name = "CreateMultipleExplorationWells")]
    public async Task<ExplorationWellDto[]> CreateMultipleExplorationWell([FromBody] ExplorationWellDto[] wellDtos)
    {
        return await _explorationWellService.CreateMultipleExplorationWells(wellDtos);
    }

    [HttpPut("multiple", Name = "UpdateMultipleExplorationWells")]
    public async Task<ExplorationWellDto[]> UpdateMultipleExplorationWell([FromQuery] Guid caseId, [FromBody] ExplorationWellDto[] wellDtos)
    {
        return await _explorationWellService.UpdateMultpleExplorationWells(wellDtos, caseId);
    }
}
