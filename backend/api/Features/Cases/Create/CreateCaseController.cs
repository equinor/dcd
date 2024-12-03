using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.FeatureToggles;
using api.Features.Projects.GetWithAssets;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Cases.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateCaseController(CreateCaseService createCaseService, GetProjectWithAssetsService getProjectWithAssetsService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/cases")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<ProjectWithAssetsDto> CreateCase([FromRoute] Guid projectId, [FromBody] CreateCaseDto caseDto)
    {
        if (FeatureToggleService.RevisionEnabled)
        {
            CreateCaseDtoValidator.Validate(caseDto);
        }

        await createCaseService.CreateCase(projectId, caseDto);

        return await getProjectWithAssetsService.GetProjectWithAssets(projectId);
    }
}
