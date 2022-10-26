using Api.Authorization;

using api.Dtos;
using api.Services;
using api.Services.GenerateCostProfiles;

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
    }

    [HttpPost("{caseId}/generateGAndGAdminCost", Name = "GenerateGAndGAdminCost")]
    public GAndGAdminCostDto GenerateGAndGAdminCost(Guid caseId)
    {
        return _generateGAndGAdminCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/calculateOpex", Name = "CalculateOpex")]
    public OpexCostProfileDto GenerateOPEX(Guid caseId)
    {
        return _generateOpexCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/calculateStudy", Name = "CalculateStudy")]
    public StudyCostProfileDto CalculateStudyCost(Guid caseId)
    {
        return _generateStudyCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateCessation", Name = "GenerateCessation")]
    public CessationCostDto GenerateCessation(Guid caseId)
    {
        return _generateCessationCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/generateNetSaleGas", Name = "GenerateNetSaleGas")]
    public NetSalesGasDto GenerateNetSaleGas(Guid caseId)
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
}
