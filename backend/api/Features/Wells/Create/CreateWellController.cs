using api.AppInfrastructure.ControllerAttributes;
using api.Features.Wells.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Wells.Create;

public class CreateWellController(CreateWellService createWellService, GetWellService getWellService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/wells")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<WellDto> CreateWell([FromRoute] Guid projectId, [FromBody] CreateWellDto wellDto)
    {
        var wellId = await createWellService.CreateWell(projectId, wellDto);
        return await getWellService.GetWell(wellId);
    }
}
