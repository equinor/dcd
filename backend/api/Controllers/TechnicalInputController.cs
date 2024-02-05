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
public class TechnicalInputController : ControllerBase
{
    private readonly ITechnicalInputService _technicalInputService;

    public TechnicalInputController(ITechnicalInputService technicalInputService)
    {
        _technicalInputService = technicalInputService;
    }

    [HttpPut(Name = "UpdateTechnicalInput")]
    public async Task<TechnicalInputDto> UpdateTechnicalInput([FromBody] TechnicalInputDto dto)
    {
        return await _technicalInputService.UpdateTehnicalInput(dto);
    }
}
