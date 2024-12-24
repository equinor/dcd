using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Revisions.Create;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CreateRevisionController(CreateRevisionService createRevisionService, GetProjectDataService getProjectDataService) : ControllerBase
{
    [HttpPost("projects/{projectId:guid}/revisions")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [DisableLazyLoading]
    public async Task<RevisionDataDto> CreateRevision([FromRoute] Guid projectId, [FromBody] CreateRevisionDto createRevisionDto)
    {
        CreateRevisionDtoValidator.Validate(createRevisionDto);

        var revisionId = await createRevisionService.CreateRevision(projectId, createRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
