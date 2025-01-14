using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Dtos.Update;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Profiles;

[Route("projects/{projectId}/cases/{caseId}/onshorePowerSupplys")]
[AuthorizeActionType(ActionType.Edit)]
public class OnshorePowerSupplyProfilesController(OnshorePowerSupplyTimeSeriesService onshorePowerSupplyTimeSeriesService) : ControllerBase
{
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
