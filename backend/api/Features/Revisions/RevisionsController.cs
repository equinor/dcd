using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectData;
using api.Features.ProjectData.Dtos;
using api.Features.Revisions.Create;
using api.Features.Revisions.Update;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Revisions;

[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class RevisionsController(
    CreateRevisionService createRevisionService,
    GetProjectDataService getProjectDataService,
    UpdateRevisionService updateRevisionService) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [ActionType(ActionType.Read)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    public async Task<RevisionDataDto> Get(Guid projectId, Guid revisionId)
    {
        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }

    [HttpPost("projects/{projectId:guid}/revisions")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<RevisionDataDto> CreateRevision([FromRoute] Guid projectId, [FromBody] CreateRevisionDto createRevisionDto)
    {
        CreateRevisionDtoValidator.Validate(createRevisionDto);

        var revisionId = await createRevisionService.CreateRevision(projectId, createRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }

    [HttpPut("projects/{projectId:guid}/revisions/{revisionId:guid}")]
    [ActionType(ActionType.Edit)]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    public async Task<RevisionDataDto> UpdateRevision([FromRoute] Guid projectId, [FromRoute] Guid revisionId, [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        await updateRevisionService.UpdateRevision(projectId, revisionId, updateRevisionDto);

        return await getProjectDataService.GetRevisionData(projectId, revisionId);
    }
}
