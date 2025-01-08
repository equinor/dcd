using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Cases.CaseComparison;

public class CaseComparisonController(CaseComparisonService caseComparisonService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/case-comparison")]
    [AuthorizeActionType(ActionType.Read)]
    [DisableLazyLoading]
    public async Task<List<CompareCasesDto>> CaseComparison(Guid projectId)
    {
        return await caseComparisonService.Calculate(projectId);
    }
}
