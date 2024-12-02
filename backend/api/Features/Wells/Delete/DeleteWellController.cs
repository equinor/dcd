using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Wells.Delete;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteWellController(DeleteWellService deleteWellService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/wells/{wellId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task DeleteWell([FromRoute] Guid projectId, [FromRoute] Guid wellId)
    {
        await deleteWellService.DeleteWell(projectId, wellId);
    }
}
