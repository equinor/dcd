using api.AppInfrastructure.Authorization;
using api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.CaseComparison;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CaseComparisonController(CaseComparisonService caseComparisonService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/case-comparison")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return await caseComparisonService.Calculate(projectId);
    }
}
