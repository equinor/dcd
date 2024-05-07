using System.Net;

using api.Dtos;
using api.Exceptions;
using api.Services;
using api.Services.GenerateCostProfiles;

using api.Authorization;

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
    private readonly IGenerateCessationCostProfile _generateCessationCostProfile;
    private readonly IGenerateOpexCostProfile _generateOpexCostProfile;
    private readonly IGenerateStudyCostProfile _generateStudyCostProfile;
    private readonly IGenerateCo2IntensityProfile _generateCo2IntensityProfile;
    private readonly IGenerateCo2IntensityTotal _generateCo2IntensityTotal;
    private readonly IGenerateCo2DrillingFlaringFuelTotals _generateCo2DrillingFlaringFuelTotals;

    public CaseGeneratedProfileController(IGenerateStudyCostProfile generateStudyCostProfile,
        IGenerateOpexCostProfile generateOpexCostProfile, IGenerateCessationCostProfile generateCessationCostProfile,
        IGenerateCo2IntensityProfile generateCo2IntensityProfile, IGenerateCo2IntensityTotal generateCo2IntensityTotal,
        IGenerateCo2DrillingFlaringFuelTotals generateCo2DrillingFlaringFuelTotals)
    {
        _generateStudyCostProfile = generateStudyCostProfile;
        _generateOpexCostProfile = generateOpexCostProfile;
        _generateCessationCostProfile = generateCessationCostProfile;
        _generateCo2IntensityProfile = generateCo2IntensityProfile;
        _generateCo2IntensityTotal = generateCo2IntensityTotal;
        _generateCo2DrillingFlaringFuelTotals = generateCo2DrillingFlaringFuelTotals;
    }

    [HttpGet("opex")]
    [ProducesResponseType(typeof(OpexCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OpexCostProfileWrapperDto>> GenerateOPEX(Guid caseId)
    {
        try
        {
            var dto = await _generateOpexCostProfile.Generate(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpGet("study")]
    [ProducesResponseType(typeof(StudyCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<StudyCostProfileWrapperDto>> GenerateStudy(Guid caseId)
    {
        try
        {
            var dto = await _generateStudyCostProfile.Generate(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpGet("cessation")]
    [ProducesResponseType(typeof(CessationCostWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CessationCostWrapperDto>> GenerateCessation(Guid caseId)
    {
        try
        {
            var dto = await _generateCessationCostProfile.Generate(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
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
