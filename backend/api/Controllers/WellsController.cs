
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

    [HttpDelete("{wellId}", Name = "DeleteWell")]
    public async Task<ProjectDto> DeleteWell(Guid wellId)
    {
        return await _wellService.DeleteWell(wellId);
    }
}
