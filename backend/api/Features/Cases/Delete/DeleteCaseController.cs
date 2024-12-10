using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Delete;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteCaseController(DeleteCaseService deleteCaseService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectDataDto> DeleteCase([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        await deleteCaseService.DeleteCase(projectId, caseId);

        return await getProjectDataService.GetProjectData(projectId);
    }
}
