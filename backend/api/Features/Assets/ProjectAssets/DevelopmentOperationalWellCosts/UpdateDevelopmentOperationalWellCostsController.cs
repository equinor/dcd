using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;

public class UpdateDevelopmentOperationalWellCostsController(UpdateDevelopmentOperationalWellCostsService updateDevelopmentOperationalWellCostsService) : ControllerBase
{
    [HttpPut("project/{projectId:guid}/development-operational-well-costs/{developmentOperationalWellCostsId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<DevelopmentOperationalWellCostsDto> UpdateDevelopmentOperationalWellCosts([FromRoute] Guid projectId, [FromRoute] Guid developmentOperationalWellCostsId, [FromBody] UpdateDevelopmentOperationalWellCostsDto dto)
    {
        return await updateDevelopmentOperationalWellCostsService.UpdateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, dto);
    }
}
