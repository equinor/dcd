using System.Net;

using api.Dtos;
using api.Services;
using api.Services.GenerateCostProfiles;

using Api.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[RequiresApplicationRoles(
    ApplicationRole.Admin,
    ApplicationRole.ReadOnly,
    ApplicationRole.User
)]
public class GenerateProfileController : ControllerBase
{
    private readonly IGenerateCessationCostProfile _generateCessationCostProfile;
    private readonly IGenerateCo2EmissionsProfile _generateCo2EmissionsProfile;
    private readonly IGenerateFuelFlaringLossesProfile _generateFuelFlaringLossessProfile;
    private readonly IGenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly IGenerateImportedElectricityProfile _generateImportedElectricityProfile;
    private readonly IGenerateNetSaleGasProfile _generateNetSaleGasProfile;
    private readonly IGenerateOpexCostProfile _generateOpexCostProfile;
    private readonly IGenerateStudyCostProfile _generateStudyCostProfile;
    private readonly IGenerateCo2IntensityProfile _generateCo2IntensityProfile;
    private readonly IGenerateCo2IntensityTotal _generateCo2IntensityTotal;
    private readonly IGenerateCo2DrillingFlaringFuelTotals _generateCo2DrillingFlaringFuelTotals;

    public GenerateProfileController(IGenerateGAndGAdminCostProfile generateGAndGAdminCostProfile, IGenerateStudyCostProfile generateStudyCostProfile,
        IGenerateOpexCostProfile generateOpexCostProfile, IGenerateCessationCostProfile generateCessationCostProfile,
        IGenerateCo2EmissionsProfile generateCo2EmissionsProfile, IGenerateNetSaleGasProfile generateNetSaleGasProfile,
        IGenerateFuelFlaringLossesProfile generateFuelFlaringLossesProfile, IGenerateImportedElectricityProfile generateImportedElectricityProfile,
        IGenerateCo2IntensityProfile generateCo2IntensityProfile, IGenerateCo2IntensityTotal generateCo2IntensityTotal,
        IGenerateCo2DrillingFlaringFuelTotals generateCo2DrillingFlaringFuelTotals)
    {
        _generateGAndGAdminCostProfile = generateGAndGAdminCostProfile;
        _generateStudyCostProfile = generateStudyCostProfile;
        _generateOpexCostProfile = generateOpexCostProfile;
        _generateCessationCostProfile = generateCessationCostProfile;
        _generateCo2EmissionsProfile = generateCo2EmissionsProfile;
        _generateNetSaleGasProfile = generateNetSaleGasProfile;
        _generateFuelFlaringLossessProfile = generateFuelFlaringLossesProfile;
        _generateImportedElectricityProfile = generateImportedElectricityProfile;
        _generateCo2IntensityProfile = generateCo2IntensityProfile;
        _generateCo2IntensityTotal = generateCo2IntensityTotal;
        _generateCo2DrillingFlaringFuelTotals = generateCo2DrillingFlaringFuelTotals;
    }

    [HttpPost("{caseId}/generateGAndGAdminCost", Name = "GenerateGAndGAdminCost")]
    public async Task<ActionResult<GAndGAdminCostDto>> GenerateGAndGAdminCostAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateGAndGAdminCostProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateOpex", Name = "GenerateOpex")]
    [ProducesResponseType(typeof(OpexCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OpexCostProfileWrapperDto>> GenerateOPEXAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateOpexCostProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateStudy", Name = "GenerateStudy")]
    [ProducesResponseType(typeof(StudyCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<StudyCostProfileWrapperDto>> GenerateStudyAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateStudyCostProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateCessation", Name = "GenerateCessation")]
    [ProducesResponseType(typeof(CessationCostWrapperDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CessationCostWrapperDto>> GenerateCessationAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateCessationCostProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateNetSaleGas", Name = "GenerateNetSaleGas")]
    public async Task<ActionResult<NetSalesGasDto>> GenerateNetSaleGasAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateNetSaleGasProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateFuelFlaringLosses", Name = "GenerateFuelFlaringLosses")]
    public async Task<ActionResult<FuelFlaringAndLossesDto>> GenerateFuelFlaringLossesAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateFuelFlaringLossessProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateCo2Emissions", Name = "GenerateCo2Emissions")]
    public async Task<ActionResult<Co2EmissionsDto>> GenerateCo2EmissionsAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateCo2EmissionsProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateImportedElectricity", Name = "GenerateImportedElectricity")]
    public async Task<ActionResult<ImportedElectricityDto>> GenerateImportedElectricityAsync(Guid caseId)
    {
        try
        {
            var dto = await _generateImportedElectricityProfile.GenerateAsync(caseId);
            return Ok(dto);
        }
        catch (NotFoundInDBException)
        {
            return NotFound();
        }
    }

    [HttpPost("{caseId}/generateCo2Intensity", Name = "GenerateCo2Intensity")]
    public async Task<Co2IntensityDto> GenerateCo2Intensity(Guid caseId)
    {
        return await _generateCo2IntensityProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateCo2IntensityTotal", Name = "GenerateCo2IntensityTotal")]
    public async Task<double> GenerateCo2IntensityTotal(Guid caseId)
    {
        return await _generateCo2IntensityTotal.Calculate(caseId);
    }

    [HttpPost("{caseId}/generateCo2DrillingFlaringFuelTotals", Name = "GenerateCo2DrillingFlaringFuelTotals")]
    public async Task<Co2DrillingFlaringFuelTotalsDto> GenerateCo2DrillingFlaringFuelTotals(Guid caseId)
    {
        return await _generateCo2DrillingFlaringFuelTotals.Generate(caseId);
    }
}
