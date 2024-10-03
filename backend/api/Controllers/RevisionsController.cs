using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/revisions")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class RevisionsController : ControllerBase
{
    private readonly IRevisionService _revisionService;

    public RevisionsController(
        IRevisionService revisionService
    )
    {
        _revisionService = revisionService;
    }

    [HttpGet("{revisionId}")]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Read)]
    public async Task<ProjectWithAssetsDto?> Get(Guid projectId, Guid revisionId)
    {
        // TODO: Need to verify that the project from the URL is the same as the project of the resource
        return await _revisionService.GetRevision(revisionId);
    }

    [HttpPost]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
    public async Task<string> CreateProject([FromRoute] Guid projectId)
    {
        return await _revisionService.CreateRevision(projectId);
    }
}
