
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

    private readonly WellService _wellService;

    public WellsController(WellService wellService)
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
}
