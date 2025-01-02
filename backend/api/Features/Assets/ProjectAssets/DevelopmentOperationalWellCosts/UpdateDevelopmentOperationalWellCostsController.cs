using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateDevelopmentOperationalWellCostsController(UpdateDevelopmentOperationalWellCostsService updateDevelopmentOperationalWellCostsService) : ControllerBase
{
    [HttpPut("project/{projectId:guid}/development-operational-well-costs/{developmentOperationalWellCostsId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid developmentOperationalWellCostsId, [FromBody] UpdateDevelopmentOperationalWellCostsDto dto)
    {
        return await updateDevelopmentOperationalWellCostsService.UpdateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, dto);
    }
}
