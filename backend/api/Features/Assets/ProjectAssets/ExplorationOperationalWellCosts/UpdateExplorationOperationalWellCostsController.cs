using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateExplorationOperationalWellCostsController(UpdateExplorationOperationalWellCostsService updateExplorationOperationalWellCostsService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/exploration-operational-well-costs/{explorationOperationalWellCostsId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<ExplorationOperationalWellCostsDto> UpdateExplorationOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid explorationOperationalWellCostsId, [FromBody] UpdateExplorationOperationalWellCostsDto dto)
    {
        return await updateExplorationOperationalWellCostsService.UpdateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, dto);
    }
}
