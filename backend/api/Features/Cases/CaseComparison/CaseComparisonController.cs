using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.CaseComparison;

[ApiController]
[Route("projects")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CaseComparisonController(CaseComparisonService caseComparisonService)
{
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet("{projectId:guid}/case-comparison")]
    [ActionType(ActionType.Read)]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return await caseComparisonService.Calculate(projectId);
    }
}
