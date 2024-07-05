using System.Net;

using api.Authorization;
using api.Dtos;
using api.Exceptions;
using api.Services;
using api.Services.GenerateCostProfiles;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("/projects/{projectId}/cases/{caseId}")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class CaseGeneratedProfileController : ControllerBase
{
    private readonly ICo2IntensityProfileService _generateCo2IntensityProfile;
    private readonly ICo2IntensityTotalService _generateCo2IntensityTotal;
    private readonly ICo2DrillingFlaringFuelTotalsService _generateCo2DrillingFlaringFuelTotals;

    public CaseGeneratedProfileController(
        ICo2IntensityProfileService generateCo2IntensityProfile,
        ICo2IntensityTotalService generateCo2IntensityTotal,
        ICo2DrillingFlaringFuelTotalsService generateCo2DrillingFlaringFuelTotals)
    {
        _generateCo2IntensityProfile = generateCo2IntensityProfile;
        _generateCo2IntensityTotal = generateCo2IntensityTotal;
        _generateCo2DrillingFlaringFuelTotals = generateCo2DrillingFlaringFuelTotals;
    }

    [HttpGet("co2Intensity")]
    public async Task<Co2IntensityDto> GenerateCo2Intensity(Guid caseId)
    {
        return await _generateCo2IntensityProfile.Generate(caseId);
    }

    [HttpGet("co2IntensityTotal")]
    public async Task<double> GenerateCo2IntensityTotal(Guid caseId)
    {
        return await _generateCo2IntensityTotal.Calculate(caseId);
    }

    [HttpGet("co2DrillingFlaringFuelTotals")]
    public async Task<Co2DrillingFlaringFuelTotalsDto> GenerateCo2DrillingFlaringFuelTotals(Guid caseId)
    {
        return await _generateCo2DrillingFlaringFuelTotals.Generate(caseId);
    }
}
