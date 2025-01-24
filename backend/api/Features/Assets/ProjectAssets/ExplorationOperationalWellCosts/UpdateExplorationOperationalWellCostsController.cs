using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts;

public class UpdateExplorationOperationalWellCostsController(UpdateExplorationOperationalWellCostsService updateExplorationOperationalWellCostsService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/exploration-operational-well-costs/{explorationOperationalWellCostsId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<ExplorationOperationalWellCostsOverviewDto> UpdateExplorationOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid explorationOperationalWellCostsId, [FromBody] UpdateExplorationOperationalWellCostsDto dto)
    {
        return await updateExplorationOperationalWellCostsService.UpdateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, dto);
    }
}
