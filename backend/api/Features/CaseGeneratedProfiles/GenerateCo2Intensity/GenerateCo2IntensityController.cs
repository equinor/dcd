using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.CaseGeneratedProfiles.GenerateCo2Intensity;

public class GenerateCo2IntensityController(Co2IntensityProfileService generateCo2IntensityProfile) : ControllerBase
{
    [AuthorizeActionType(ActionType.Read)]
    [HttpGet("projects/{projectId:guid}/cases/{caseId:guid}/co2Intensity")]
    public async Task<Co2IntensityDto> GenerateCo2Intensity(Guid projectId, Guid caseId)
    {
        return await generateCo2IntensityProfile.Generate(caseId);
    }
}
