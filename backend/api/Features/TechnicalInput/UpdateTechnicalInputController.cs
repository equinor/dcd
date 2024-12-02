using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.TechnicalInput;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateTechnicalInputController(TechnicalInputService technicalInputService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/technical-input")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<TechnicalInputDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] UpdateTechnicalInputDto dto)
    {
        return await technicalInputService.UpdateTechnicalInput(projectId, dto);
    }
}
