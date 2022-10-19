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
    private readonly GenerateEmissionsProfile _generateEmissionsProfile;
    private readonly GenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly GenerateOpexCostProfile _generateOpexCostProfile;
    private readonly GenerateStudyCostProfile _generateStudyCostProfile;

    public GenerateProfileController(IServiceProvider serviceProvider)
    {
        _generateGAndGAdminCostProfile = serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>();
        _generateStudyCostProfile = serviceProvider.GetRequiredService<GenerateStudyCostProfile>();
        _generateOpexCostProfile = serviceProvider.GetRequiredService<GenerateOpexCostProfile>();
        _generateCessationCostProfile = serviceProvider.GetRequiredService<GenerateCessationCostProfile>();
        _generateEmissionsProfile = serviceProvider.GetRequiredService<GenerateEmissionsProfile>();
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
        return _generateEmissionsProfile.GenerateNetSaleGas(caseId);
    }

    [HttpPost("{caseId}/generateFuelFlaringAndLosses", Name = "GenerateFuelFlaringAndLosses")]
    public FuelFlaringAndLossesDto GenerateFuelFlaringAndLosses(Guid caseId)
    {
        return _generateEmissionsProfile.GenerateFuelFlaringAndLosses(caseId);
    }

    [HttpPost("{caseId}/generateCo2Emissions", Name = "GenerateCo2Emissions")]
    public Co2EmissionsDto GenerateCo2Emissions(Guid caseId)
    {
        return _generateEmissionsProfile.GenerateCo2Emissions(caseId);
    }
}
