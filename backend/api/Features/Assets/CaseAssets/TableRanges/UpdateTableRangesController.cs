using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.TableRanges;

public class UpdateTableRangesController(UpdateTableRangesService updateTableRangesService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/table-ranges")]
    public async Task<NoContentResult> UpdateTableRanges(Guid projectId, Guid caseId, [FromBody] UpdateTableRangesDto dto)
    {
        await updateTableRangesService.UpdateTableRanges(projectId, caseId, dto);

        return NoContent();
    }
} 
