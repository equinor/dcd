using api.Authorization;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Models.Fusion;
using api.Services;
using api.Services.FusionIntegration;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
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

    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.ReadOnly,
        ApplicationRole.User
    )]
    [HttpGet]
    [ActionType(ActionType.Read)]
    public async Task<ProjectWithAssetsDto?> Get(Guid projectId)
    {
        return await _revisionService.GetRevision(projectId);
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

    // [RequiresApplicationRoles(
    //     ApplicationRole.Admin,
    //     ApplicationRole.User
    // )]
    // [HttpPut("{projectId}")]
    // [ActionType(ActionType.Edit)]
    // public async Task<ProjectWithCasesDto> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectDto projectDto)
    // {

    // }
}
