using api.AppInfrastructure.Authorization;
using api.Controllers;
using api.Features.Revisions.Create;
using api.Features.Revisions.Get;
using api.Features.Revisions.Update;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Features.Revisions;

[ApiController]
[Route("projects/{projectId:guid}/revisions")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class RevisionsController(
    CreateRevisionService createRevisionService,
    GetRevisionService getRevisionService,
    UpdateRevisionService updateRevisionService) : ControllerBase
{
    [HttpGet("{revisionId:guid}")]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.ReadOnly, ApplicationRole.User)]
    [ActionType(ActionType.Read)]
    public async Task<RevisionWithCasesDto> Get(Guid projectId, Guid revisionId)
    {
        return await getRevisionService.GetRevision(projectId, revisionId);
    }

    [HttpPost]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Edit)]
    public async Task<RevisionWithCasesDto> CreateRevision([FromRoute] Guid projectId, [FromBody] CreateRevisionDto createRevisionDto)
    {
        CreateRevisionDtoValidator.Validate(createRevisionDto);

        var revisionId = await createRevisionService.CreateRevision(projectId, createRevisionDto);

        return await getRevisionService.GetRevision(projectId, revisionId);
    }

    [HttpPut("{revisionId:guid}")]
    [RequiresApplicationRoles(ApplicationRole.Admin, ApplicationRole.User)]
    [ActionType(ActionType.Edit)]
    public async Task<RevisionWithCasesDto> UpdateRevision([FromRoute] Guid projectId, [FromRoute] Guid revisionId, [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        await updateRevisionService.UpdateRevision(projectId, revisionId, updateRevisionDto);

        return await getRevisionService.GetRevision(projectId, revisionId);
    }
}
