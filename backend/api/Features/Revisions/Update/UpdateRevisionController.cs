using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Revisions.Update;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UpdateRevisionController(GetProjectDataService getProjectDataService, UpdateRevisionService updateRevisionService) : ControllerBase
{
    [HttpPut("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<RevisionDataDto> UpdateRevision([FromRoute] Guid projectId, [FromRoute] Guid revisionId, [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        await updateRevisionService.UpdateRevision(projectId, revisionId, updateRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
