using api.Authorization;
using api.Dtos;
using api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[ApiController]
[Route("projects/{projectId}/revisions")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class RevisionsController(IRevisionService revisionService) : ControllerBase
{
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
        return await revisionService.GetRevision(revisionId);
    }

    [HttpPost]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
    public async Task<ProjectWithAssetsDto> CreateRevision([FromRoute] Guid projectId, [FromBody] CreateRevisionDto createRevisionDto)
    {
        // Do not enable backend validation until frontend actually displays the name field.
        // Probably done by the end of november 2024.

        //CreateRevisionDtoValidator.Validate(createRevisionDto);
        return await revisionService.CreateRevision(projectId, createRevisionDto);
    }
}
