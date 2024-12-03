using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Services.GenerateCostProfiles;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.CaseProfiles;

[ApiController]
[Route("/projects/{projectId}/cases/{caseId}")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
[ActionType(ActionType.Read)]
public class CaseGeneratedProfileController(
    ICo2IntensityProfileService generateCo2IntensityProfile,
    ICo2IntensityTotalService generateCo2IntensityTotal,
    ICo2DrillingFlaringFuelTotalsService generateCo2DrillingFlaringFuelTotals)
    : ControllerBase
{
    [HttpGet("co2Intensity")]
    public async Task<Co2IntensityDto> GenerateCo2Intensity(Guid caseId)
    {
        return await generateCo2IntensityProfile.Generate(caseId);
    }

    [HttpGet("co2IntensityTotal")]
    public async Task<double> GenerateCo2IntensityTotal(Guid caseId)
    {
        return await generateCo2IntensityTotal.Calculate(caseId);
    }

    [HttpGet("co2DrillingFlaringFuelTotals")]
    public async Task<Co2DrillingFlaringFuelTotalsDto> GenerateCo2DrillingFlaringFuelTotals(Guid caseId)
    {
        return await generateCo2DrillingFlaringFuelTotals.Generate(caseId);
    }
}
