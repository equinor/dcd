
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
public class WellsController : ControllerBase
{

    private readonly IWellService _wellService;

    public WellsController(IWellService wellService)
    {
        _wellService = wellService;
    }

    [HttpGet("{wellId}", Name = "GetWell")]
    public async Task<WellDto> GetWell(Guid wellId)
    {
        return await _wellService.GetWellDto(wellId);
    }

    [HttpGet(Name = "GetWells")]
    public async Task<IEnumerable<WellDto>> GetWells([FromQuery] Guid projectId)
    {
        if (projectId != Guid.Empty)
        {
            return await _wellService.GetDtosForProject(projectId);
        }
        return await _wellService.GetAllDtos();
    }

    [HttpPost(Name = "CreateWell")]
    public async Task<ProjectDto> CreateWell([FromBody] WellDto wellDto)
    {
        return await _wellService.CreateWell(wellDto);
    }

    [HttpPut(Name = "UpdateWell")]
    public async Task<ProjectDto> UpdateWell([FromBody] WellDto wellDto)
    {
        return await _wellService.UpdateWell(wellDto);
    }

    [HttpPut("multiple", Name = "UpdateMultipleWells")]
    public async Task<WellDto[]>? UpdateMultipleWells([FromBody] WellDto[] wellDtos)
    {
        return await _wellService.UpdateMultiple(wellDtos);
    }

    [HttpPost("multiple", Name = "CreateMultipleWells")]
    public async Task<WellDto[]>? CreateMultipleWells([FromBody] WellDto[] wellDtos)
    {
        return await _wellService.CreateMultipleWells(wellDtos);
    }

    [HttpDelete("{wellId}", Name = "DeleteWell")]
    public async Task<ProjectDto> DeleteWell(Guid wellId)
    {
        return await _wellService.DeleteWell(wellId);
    }
}
