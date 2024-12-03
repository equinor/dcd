using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.Projects.GetWithAssets;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Delete;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class DeleteCaseController(DeleteCaseService deleteCaseService, GetProjectWithAssetsService getProjectWithAssetsService) : ControllerBase
{
    [HttpDelete("projects/{projectId:guid}/cases/{caseId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectWithAssetsDto> DeleteCase([FromRoute] Guid projectId, [FromRoute] Guid caseId)
    {
        await deleteCaseService.DeleteCase(projectId, caseId);

        return await getProjectWithAssetsService.GetProjectWithAssets(projectId);
    }
}
