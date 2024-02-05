
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
    public WellDto GetWell(Guid wellId)
    {
        return _wellService.GetWellDto(wellId);
    }

    [HttpGet(Name = "GetWells")]
    public IEnumerable<WellDto> GetWells([FromQuery] Guid projectId)
    {
        if (projectId != Guid.Empty)
        {
            return _wellService.GetDtosForProject(projectId);
        }
        return _wellService.GetAllDtos();
    }

    [HttpPost(Name = "CreateWell")]
    public ProjectDto CreateWell([FromBody] WellDto wellDto)
    {
        return _wellService.CreateWell(wellDto);
    }

    [HttpPut(Name = "UpdateWell")]
    public ProjectDto UpdateWell([FromBody] WellDto wellDto)
    {
        return _wellService.UpdateWell(wellDto);
    }

    [HttpPut("multiple", Name = "UpdateMultipleWells")]
    public WellDto[]? UpdateMultipleWells([FromBody] WellDto[] wellDtos)
    {
        return _wellService.UpdateMultipleWells(wellDtos);
    }

    [HttpPost("multiple", Name = "UpdateMultipleWells")]
    public WellDto[]? CreateMultipleWells([FromBody] WellDto[] wellDtos)
    {
        return _wellService.CreateMultipleWells(wellDtos);
    }

    [HttpDelete("{wellId}", Name = "DeleteWell")]
    public ProjectDto DeleteWell(Guid wellId)
    {
        return _wellService.DeleteWell(wellId);
    }
}
