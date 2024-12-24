using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Duplicate;

[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DuplicateCaseController(DuplicateCaseService duplicateCaseService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/copy")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<ProjectDataDto> DuplicateCase([FromRoute] Guid projectId, [FromQuery] Guid copyCaseId)
    {
        await duplicateCaseService.DuplicateCase(projectId, copyCaseId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
