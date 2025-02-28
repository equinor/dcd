using api.AppInfrastructure.ControllerAttributes;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Transports;

public class UpdateTransportController(UpdateTransportService updateTransportService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/transport")]
    public async Task<NoContentResult> UpdateTransport(Guid projectId, Guid caseId, [FromBody] UpdateTransportDto dto)
    {
        await updateTransportService.UpdateTransport(projectId, caseId, dto);

        return NoContent();
    }
}
