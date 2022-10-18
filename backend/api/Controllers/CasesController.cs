
using api.Dtos;
using api.Services;

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
public class CasesController : ControllerBase
{
    private readonly CaseService _caseService;
    private readonly GenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly GenerateStudyCostProfile _generateStudyCostProfile;
    private readonly GenerateOpexCostProfile _generateOpexCostProfile;
    private readonly GenerateCessationCostProfile _generateCessationCostProfile;

    public CasesController(CaseService caseService, IServiceProvider serviceProvider)
    {
        _caseService = caseService;
        _generateGAndGAdminCostProfile = serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>();
        _generateStudyCostProfile = serviceProvider.GetRequiredService<GenerateStudyCostProfile>();
        _generateOpexCostProfile = serviceProvider.GetRequiredService<GenerateOpexCostProfile>();
        _generateCessationCostProfile = serviceProvider.GetRequiredService<GenerateCessationCostProfile>();
    }

    [HttpPost(Name = "CreateCase")]
    public ProjectDto CreateCase([FromBody] CaseDto caseDto)
    {
        return _caseService.CreateCase(caseDto);
    }

    [HttpPost("copy", Name = "Duplicate")]
    public ProjectDto DuplicateCase([FromQuery] Guid copyCaseId)
    {
        return _caseService.DuplicateCase(copyCaseId);
    }

    [HttpPut(Name = "UpdateCase")]
    public ProjectDto UpdateCase([FromBody] CaseDto caseDto)
    {
        return _caseService.UpdateCase(caseDto);
    }

    [HttpDelete("{caseId}", Name = "DeleteCase")]
    public ProjectDto DeleteTransport(Guid caseId)
    {
        return _caseService.DeleteCase(caseId);
    }

    [HttpPost("{caseId}/generateGAndGAdminCost", Name = "GenerateGAndGAdminCost")]
    public GAndGAdminCostDto GenerateGAndGAdminCost(Guid caseId)
    {
        return _generateGAndGAdminCostProfile.Generate(caseId);
    }

    [HttpPost("{caseId}/calculateOpex", Name = "CalculateOpex")]
    public OpexCostProfileDto CalculateOPEX(Guid caseId)
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
}
