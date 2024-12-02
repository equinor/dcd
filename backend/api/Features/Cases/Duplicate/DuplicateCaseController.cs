using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Duplicate;

[ApiController]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DuplicateCaseController(DuplicateCaseService duplicateCaseService, IProjectService projectService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases/copy")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectWithAssetsDto> DuplicateCase([FromRoute] Guid projectId, [FromQuery] Guid copyCaseId)
    {
        await duplicateCaseService.DuplicateCase(copyCaseId);

        return await projectService.GetProjectDto(projectId);
    }
}
