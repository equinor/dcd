using api.AppInfrastructure.Authorization;
using api.Context;
using api.Exceptions;
using api.Features.FusionIntegration.ProjectMaster;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Projects.Exists;

public class ProjectExistsService(DcdDbContext context, IFusionService fusionService, CurrentUser currentUser)
{
    private readonly List<ApplicationRole> _rolesPermittedToCreateProject = [ApplicationRole.Admin, ApplicationRole.User];
    public async Task<ProjectExistsDto> ProjectExists(Guid contextId)
    {
        var projectMaster = await fusionService.GetProjectMasterFromFusionContextId(contextId);

        if (projectMaster == null)
        {
            throw new NotFoundInDbException($"Project with context ID {contextId} not found in the external API.");
        }

        var projectExists = await context.Projects.AnyAsync(p => p.FusionProjectId == projectMaster.Identity);
        var canCreateProject = !projectExists && _rolesPermittedToCreateProject.Intersect(currentUser.ApplicationRoles).Any();

        return new ProjectExistsDto
        {
            ProjectExists = projectExists,
            CanCreateProject = canCreateProject
        };
    }
}
