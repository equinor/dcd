using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;
using api.Features.Cases.GetWithAssets;
using api.Features.Stea.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

[Route("projects/{projectId}/cases/{caseId}/onshorePowerSupplys")]
[AuthorizeActionType(ActionType.Edit)]
public class OnshorePowerSuppliesController(
    OnshorePowerSupplyService onshorePowerSupplyService,
    OnshorePowerSupplyTimeSeriesService onshorePowerSupplyTimeSeriesService) : ControllerBase
{
    [HttpPut("{onshorePowerSupplyId}")]
    public async Task<OnshorePowerSupplyDto> UpdateOnshorePowerSupply(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] APIUpdateOnshorePowerSupplyDto dto)
    {
        return await onshorePowerSupplyService.UpdateOnshorePowerSupply(projectId, caseId, onshorePowerSupplyId, dto);
    }

    [HttpPost("{onshorePowerSupplyId}/cost-profile-override")]
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromBody] CreateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.CreateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, dto);
    }

    [HttpPut("{onshorePowerSupplyId}/cost-profile-override/{costProfileId}")]
    public async Task<OnshorePowerSupplyCostProfileOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid onshorePowerSupplyId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateOnshorePowerSupplyCostProfileOverrideDto dto)
    {
        return await onshorePowerSupplyTimeSeriesService.UpdateOnshorePowerSupplyCostProfileOverride(projectId, caseId, onshorePowerSupplyId, costProfileId, dto);
    }
}
