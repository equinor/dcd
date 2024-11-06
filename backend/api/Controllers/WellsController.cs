using api.Authorization;
using api.Dtos;
using api.Services;

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
public class WellsController(IWellService wellService) : ControllerBase
{
    [HttpPut("{wellId}")]
    [ActionType(ActionType.Edit)]
    public async Task<WellDto> UpdateWell([FromRoute] Guid projectId, [FromRoute] Guid wellId, [FromBody] UpdateWellDto wellDto)
    {
        return await wellService.UpdateWell(projectId, wellId, wellDto);
    }

    [HttpPost]
    [ActionType(ActionType.Edit)]
    public async Task<WellDto> CreateWell([FromRoute] Guid projectId, [FromBody] CreateWellDto wellDto)
    {
        return await wellService.CreateWell(projectId, wellDto);
    }

    [HttpDelete("{wellId}")]
    [ActionType(ActionType.Edit)]
    public async Task DeleteWell([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        await wellService.DeleteWell(projectId, wellId);
    }

    [HttpGet("{wellId}/affected-cases")]
    [ActionType(ActionType.Edit)]
    public async Task<IEnumerable<CaseDto>> GetAffectedCases([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        return await wellService.GetAffectedCases(wellId);
    }
}
