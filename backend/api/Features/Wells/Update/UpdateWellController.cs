using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Wells.Get;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Wells.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateWellController(UpdateWellService updateWellService, GetWellService getWellService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/wells/{wellId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<WellDto> UpdateWell([FromRoute] Guid projectId, [FromRoute] Guid wellId, [FromBody] UpdateWellDto wellDto)
    {
        await updateWellService.UpdateWell(projectId, wellId, wellDto);
        return await getWellService.GetWell(wellId);
    }
}
