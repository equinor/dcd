
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
public class WellProjectWellsController : ControllerBase
{

    private readonly IWellProjectWellService _wellProjectWellService;

    public WellProjectWellsController(IWellProjectWellService wellProjectWellService)
    {
        _wellProjectWellService = wellProjectWellService;
    }

    [HttpGet(Name = "GetWellProjectWells")]
    public async Task<IEnumerable<WellProjectWellDto>> GetWellProjectWells([FromQuery] Guid projectId)
    {
        return await _wellProjectWellService.GetAllDtos();
    }

    [HttpPost(Name = "CreateWellProjectWell")]
    public async Task<ProjectDto> CreateWellProjectWell([FromBody] WellProjectWellDto wellDto)
    {
        return await _wellProjectWellService.CreateWellProjectWell(wellDto);
    }

    [HttpPut(Name = "UpdateWellProjectWell")]
    public async Task<ProjectDto> UpdateWellProjectWell([FromBody] WellProjectWellDto wellDto)
    {
        return await _wellProjectWellService.UpdateWellProjectWell(wellDto);
    }

    [HttpPost("multiple", Name = "CreateMultipleWellProjectWells")]
    public async Task<WellProjectWellDto[]> CreateMultipleWellProjectWells([FromBody] WellProjectWellDto[] wellDto)
    {
        return await _wellProjectWellService.CreateMultipleWellProjectWells(wellDto);
    }

    [HttpPut("multiple", Name = "UpdateMultipleWellProjectWells")]
    public async Task<WellProjectWellDto[]> UpdateMultipleWellProjectWells([FromQuery] Guid caseId, [FromBody] WellProjectWellDto[] wellDto)
    {
        return await _wellProjectWellService.UpdateMultipleWellProjectWells(wellDto, caseId);
    }
}
