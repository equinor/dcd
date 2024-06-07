using api.Authorization;
using api.Dtos;
using api.Services;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("projects/{projectId}/wells")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class WellsController : ControllerBase
{

    private readonly IWellService _wellService;

    public WellsController(
        IWellService wellService
    )
    {
        _wellService = wellService;
    }

    [HttpPut("{wellId}")]
    public async Task<WellDto> UpdateWell([FromRoute] Guid projectId, [FromRoute] Guid wellId, [FromBody] UpdateWellDto wellDto)
    {
        return await _wellService.UpdateWell(wellId, wellDto);
    }
}
