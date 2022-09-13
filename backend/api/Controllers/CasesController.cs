
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CasesController : ControllerBase
{
    private readonly CaseService _caseService;

    public CasesController(CaseService caseService)
    {
        _caseService = caseService;
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
        return _caseService.GenerateGAndGAdminCost(caseId);
    }

    [HttpPost("{caseId}/calculateOpex", Name = "CalculateOpex")]
    public OpexCostProfileDto CalculateOPEX(Guid caseId)
    {
        return _caseService.CalculateOPEX(caseId);
    }

    [HttpPost("{caseId}/calculateStudy", Name = "CalculateStudy")]
    public StudyCostProfileDto CalculateStudyCost(Guid caseId)
    {
        return _caseService.CalculateStudyCost(caseId);
    }
}
