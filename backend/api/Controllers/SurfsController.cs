using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class SurfsController : ControllerBase
{
    private readonly SurfService _surfService;


    public SurfsController(SurfService surfService)
    {
        _surfService = surfService;
    }

    [HttpPut(Name = "UpdateSurf")]
    public ProjectDto UpdateSurf([FromBody] SurfDto surfDto)
    {
        return _surfService.UpdateSurf(surfDto);
    }

    [HttpPost(Name = "CreateSurf")]
    public ProjectDto CreateSurf([FromQuery] Guid sourceCaseId, [FromBody] SurfDto surfDto)
    {
        return _surfService.CreateSurf(surfDto, sourceCaseId);
    }

    [HttpDelete("{surfId}", Name = "DeleteSurf")]
    public ProjectDto DeleteSurf(Guid surfId)
    {
        return _surfService.DeleteSurf(surfId);
    }
}
