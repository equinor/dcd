using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.TechnicalInput.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.TechnicalInput;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateTechnicalInputController(TechnicalInputService technicalInputService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/technical-input")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectDataDto> UpdateTechnicalInput([FromRoute] Guid projectId, [FromBody] UpdateTechnicalInputDto dto)
    {
        await technicalInputService.UpdateTechnicalInput(projectId, dto);
        return await getProjectDataService.GetProjectData(projectId);
    }
}
