using api.AppInfrastructure.ControllerAttributes;
using api.Features.Profiles.Explorations.CountryOfficeCosts.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Explorations.CountryOfficeCosts;

public class CountryOfficeCostController(CountryOfficeCostService service) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPost("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/country-office-cost")]
    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromBody] CreateCountryOfficeCostDto dto)
    {
        return await service.CreateCountryOfficeCost(projectId, caseId, explorationId, dto);
    }

    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/explorations/{explorationId:guid}/country-office-cost/{costProfileId:guid}")]
    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid explorationId,
        [FromRoute] Guid costProfileId,
        [FromBody] UpdateCountryOfficeCostDto dto)
    {
        return await service.UpdateCountryOfficeCost(projectId, caseId, explorationId, costProfileId, dto);
    }
}
