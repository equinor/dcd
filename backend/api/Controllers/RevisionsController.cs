using api.Authorization;
using api.Dtos.Project.Revision;
using api.Services;
using api.StartupConfiguration.FeatureToggles;

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
    public async Task<RevisionWithCasesDto?> Get(Guid projectId, Guid revisionId)
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
    public async Task<RevisionWithCasesDto> CreateRevision([FromRoute] Guid projectId,
        [FromBody] CreateRevisionDto createRevisionDto)
    {
        if (FeatureToggleService.RevisionEnabled)
        {
            CreateRevisionDtoValidator.Validate(createRevisionDto);
        }

        return await revisionService.CreateRevision(projectId, createRevisionDto);
    }

    [HttpPut("{revisionId}")]
    [RequiresApplicationRoles(
        ApplicationRole.Admin,
        ApplicationRole.User
    )]
    [ActionType(ActionType.Edit)]
    public async Task<RevisionWithCasesDto> UpdateRevision([FromRoute] Guid projectId, [FromRoute] Guid revisionId,
        [FromBody] UpdateRevisionDto updateRevisionDto)
    {
        return await revisionService.UpdateRevision(projectId, revisionId, updateRevisionDto);
    }
}
