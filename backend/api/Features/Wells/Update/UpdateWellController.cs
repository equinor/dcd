using api.AppInfrastructure.ControllerAttributes;
using api.Features.Wells.Get;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Wells.Update;


public class UpdateWellController(UpdateWellService updateWellService, GetWellService getWellService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/wells/{wellId:guid}")]
    [AuthorizeActionType(ActionType.Edit)]
    [DisableLazyLoading]
    public async Task<WellDto> UpdateWell([FromRoute] Guid projectId, [FromRoute] Guid wellId, [FromBody] UpdateWellDto wellDto)
    {
        await updateWellService.UpdateWell(projectId, wellId, wellDto);
        return await getWellService.GetWell(wellId);
    }
}
