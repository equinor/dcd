using api.AppInfrastructure.ControllerAttributes;
using api.Features.Cases.GetWithAssets;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Transports;

public class UpdateTransportController(UpdateTransportService updateTransportService) : ControllerBase
{
    [AuthorizeActionType(ActionType.Edit)]
    [HttpPut("projects/{projectId:guid}/cases/{caseId:guid}/transports/{transportId:guid}")]
    public async Task<TransportDto> UpdateTransport(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] UpdateTransportDto dto)
    {
        return await updateTransportService.UpdateTransport(projectId, caseId, transportId, dto);
    }
}
