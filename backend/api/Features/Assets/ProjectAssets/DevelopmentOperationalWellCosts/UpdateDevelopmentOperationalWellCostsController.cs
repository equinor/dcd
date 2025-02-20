using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts;

public class UpdateDevelopmentOperationalWellCostsController(UpdateDevelopmentOperationalWellCostsService updateDevelopmentOperationalWellCostsService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/development-operational-well-costs/{developmentOperationalWellCostsId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    public async Task<DevelopmentOperationalWellCostsOverviewDto> UpdateDevelopmentOperationalWellCosts(Guid projectId, Guid developmentOperationalWellCostsId, [FromBody] UpdateDevelopmentOperationalWellCostsDto dto)
    {
        return await updateDevelopmentOperationalWellCostsService.UpdateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, dto);
    }
}
