using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profiles.Cases.GeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;

public class GenerateCo2DrillingFlaringFuelTotalsController(Co2DrillingFlaringFuelTotalsService generateCo2DrillingFlaringFuelTotals) : ControllerBase
{
    [AuthorizeActionType(ActionType.Read)]
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/co2DrillingFlaringFuelTotals")]
    public async Task<Co2DrillingFlaringFuelTotalsDto> GenerateCo2DrillingFlaringFuelTotals(Guid projectId, Guid caseId)
    {
        return await generateCo2DrillingFlaringFuelTotals.Generate(caseId);
    }
}
