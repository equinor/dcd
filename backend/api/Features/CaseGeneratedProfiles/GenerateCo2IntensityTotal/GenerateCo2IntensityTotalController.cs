using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseGeneratedProfiles.GenerateCo2IntensityTotal;

public class GenerateCo2IntensityTotalController(Co2IntensityTotalService generateCo2IntensityTotal) : ControllerBase
{
    [AuthorizeActionType(ActionType.Read)]
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/co2IntensityTotal")]
    public async Task<double> GenerateCo2IntensityTotal(Guid projectId, Guid caseId)
    {
        return await generateCo2IntensityTotal.Calculate(caseId);
    }
}
