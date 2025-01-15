using api.AppInfrastructure.ControllerAttributes;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Services;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Assets.CaseAssets.Transports;

[Route("projects/{projectId}/cases/{caseId}/transports")]
[AuthorizeActionType(ActionType.Edit)]
public class TransportsController(TransportService transportService) : ControllerBase
{
    [HttpPut("{transportId}")]
    public async Task<TransportDto> UpdateTransport(
        [FromRoute] Guid projectId,
        [FromRoute] Guid caseId,
        [FromRoute] Guid transportId,
        [FromBody] APIUpdateTransportDto dto)
    {
        return await transportService.UpdateTransport(projectId, caseId, transportId, dto);
    }
}
