
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
    public IEnumerable<ExplorationWellDto> GetExplorationWells([FromQuery] Guid projectId)
    {
        return _explorationWellService.GetAllDtos();
    }

    [HttpPost(Name = "CreateExplorationWell")]
    public ProjectDto CreateExplorationWell([FromBody] ExplorationWellDto wellDto)
    {
        return _explorationWellService.CreateExplorationWell(wellDto);
    }

    [HttpPut(Name = "UpdateExplorationWell")]
    public ProjectDto UpdateExplorationWell([FromBody] ExplorationWellDto wellDto)
    {
        return _explorationWellService.UpdateExplorationWell(wellDto);
    }

    [HttpPost("multiple", Name = "CreateMultipleExplorationWells")]
    public ExplorationWellDto[]? CreateMultipleExplorationWell([FromBody] ExplorationWellDto[] wellDtos)
    {
        return _explorationWellService.CreateMultipleExplorationWells(wellDtos);
    }

    [HttpPut("multiple", Name = "UpdateMultipleExplorationWells")]
    public ExplorationWellDto[]? UpdateMultipleExplorationWell([FromQuery] Guid caseId, [FromBody] ExplorationWellDto[] wellDtos)
    {
        return _explorationWellService.UpdateMultpleExplorationWells(wellDtos, caseId);
    }
}
