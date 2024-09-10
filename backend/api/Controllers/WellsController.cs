using api.Authorization;
using api.Dtos;
using api.Services;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/wells")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
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
        return await _wellService.UpdateWell(projectId, wellId, wellDto);
    }

    [HttpPost]
    public async Task<WellDto> CreateWell([FromRoute] Guid projectId, [FromBody] CreateWellDto wellDto)
    {
        return await _wellService.CreateWell(projectId, wellDto);
    }

    [HttpDelete("{wellId}")]
    public async Task DeleteWell([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        await _wellService.DeleteWell(projectId, wellId);
    }

    [HttpGet("{wellId}/affected-cases")]
    public async Task<IEnumerable<CaseDto>> GetAffectedCases([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        return await _wellService.GetAffectedCases(wellId);
    }
}
