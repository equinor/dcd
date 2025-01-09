using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Context.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace api.Features.ProjectAccess;

public class ProjectAccessController(IProjectAccessService projectAccessService, DcdDbContext context) : ControllerBase
{
    [HttpGet("projects/{projectId:guid}/access")]
    [DisableLazyLoading]
    public async Task<AccessRightsDto> GetAccess(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        return await projectAccessService.GetUserProjectAccess(projectPk);
    }
}
