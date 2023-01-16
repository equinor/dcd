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
    private readonly GenerateCessationCostProfile _generateCessationCostProfile;
    private readonly GenerateCo2EmissionsProfile _generateCo2EmissionsProfile;
    private readonly GenerateFuelFlaringLossesProfile _generateFuelFlaringLossessProfile;
    private readonly GenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly GenerateImportedElectricityProfile _generateImportedElectricityProfile;
    private readonly GenerateNetSaleGasProfile _generateNetSaleGasProfile;
    private readonly GenerateOpexCostProfile _generateOpexCostProfile;
    private readonly GenerateStudyCostProfile _generateStudyCostProfile;
    private readonly GenerateCo2IntensityProfile _generateCo2IntensityProfile;
    private readonly GenerateCo2IntensityTotal _generateCo2IntensityTotal;
    private readonly GenerateCo2DrillingFlaringFuelTotals _generateCo2DrillingFlaringFuelTotals;

    public GenerateProfileController(IServiceProvider serviceProvider)
    {
        _generateGAndGAdminCostProfile = serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>();
        _generateStudyCostProfile = serviceProvider.GetRequiredService<GenerateStudyCostProfile>();
        _generateOpexCostProfile = serviceProvider.GetRequiredService<GenerateOpexCostProfile>();
        _generateCessationCostProfile = serviceProvider.GetRequiredService<GenerateCessationCostProfile>();
        _generateCo2EmissionsProfile = serviceProvider.GetRequiredService<GenerateCo2EmissionsProfile>();
        _generateNetSaleGasProfile = serviceProvider.GetRequiredService<GenerateNetSaleGasProfile>();
        _generateFuelFlaringLossessProfile = serviceProvider.GetRequiredService<GenerateFuelFlaringLossesProfile>();
        _generateImportedElectricityProfile = serviceProvider.GetRequiredService<GenerateImportedElectricityProfile>();
        _generateCo2IntensityProfile = serviceProvider.GetRequiredService<GenerateCo2IntensityProfile>();
        _generateCo2IntensityTotal = serviceProvider.GetRequiredService<GenerateCo2IntensityTotal>();
        _generateCo2DrillingFlaringFuelTotals = serviceProvider.GetRequiredService<GenerateCo2DrillingFlaringFuelTotals>();
    }

    [HttpPost("{caseId}/generateGAndGAdminCost", Name = "GenerateGAndGAdminCost")]
    public GAndGAdminCostDto GenerateGAndGAdminCost(Guid caseId)
    {
        return _generateGAndGAdminCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateOpex", Name = "GenerateOpex")]
    [ProducesResponseType(typeof(OpexCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public IActionResult GenerateOPEX(Guid caseId)
    {
        return Ok(_generateOpexCostProfile.Generate(caseId));
    }

    [HttpPost("{caseId}/generateStudy", Name = "GenerateStudy")]
    [ProducesResponseType(typeof(StudyCostProfileWrapperDto), (int)HttpStatusCode.OK)]
    public IActionResult CalculateStudyCost(Guid caseId)
    {
        return Ok(_generateStudyCostProfile.Generate(caseId));
    }

    [HttpPost("{caseId}/generateCessation", Name = "GenerateCessation")]
    [ProducesResponseType(typeof(CessationCostWrapperDto), (int)HttpStatusCode.OK)]
    public IActionResult GenerateCessation(Guid caseId)
    {
        return Ok(_generateCessationCostProfile.Generate(caseId));
    }

    [HttpPost("{caseId}/generateNetSaleGas", Name = "GenerateNetSaleGas")]
    public NetSalesGasDto? GenerateNetSaleGas(Guid caseId)
    {
        return _generateNetSaleGasProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateFuelFlaringLosses", Name = "GenerateFuelFlaringLosses")]
    public FuelFlaringAndLossesDto GenerateFuelFlaringLosses(Guid caseId)
    {
        return _generateFuelFlaringLossessProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateCo2Emissions", Name = "GenerateCo2Emissions")]
    public Co2EmissionsDto GenerateCo2Emissions(Guid caseId)
    {
        return _generateCo2EmissionsProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateImportedElectricity", Name = "GenerateImportedElectricity")]
    public ImportedElectricityDto GenerateImportedElectricity(Guid caseId)
    {
        return _generateImportedElectricityProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateCo2Intensity", Name = "GenerateCo2Intensity")]
    public Co2IntensityDto GenerateCo2Intensity(Guid caseId)
    {
        return _generateCo2IntensityProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateCo2IntensityTotal", Name = "GenerateCo2IntensityTotal")]
    public double GenerateCo2IntensityTotal(Guid caseId)
    {
        return _generateCo2IntensityTotal.Calculate(caseId);
    }

    [HttpPost("{caseId}/generateCo2DrillingFlaringFuelTotals", Name = "GenerateCo2DrillingFlaringFuelTotals")]
    public Co2DrillingFlaringFuelTotalsDto GenerateCo2DrillingFlaringFuelTotals(Guid caseId)
    {
        return _generateCo2DrillingFlaringFuelTotals.Generate(caseId);
    }
}
