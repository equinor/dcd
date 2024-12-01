using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Wells.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Wells.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateWellController(CreateWellService createWellService, GetWellService getWellService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/wells")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<WellDto> CreateWell([FromRoute] Guid projectId, [FromBody] CreateWellDto wellDto)
    {
        var wellId = await createWellService.CreateWell(projectId, wellDto);
        return await getWellService.GetWell(wellId);
    }
}
