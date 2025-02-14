using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Exceptions;
using api.Features.FusionIntegration.ProjectMaster;
using api.Features.ProjectAccess;
using api.Models.Enums;

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
            CanCreateProject = canCreateProject,
            NoAccessReason = await CalculateNoAccessReason(projectExists, projectMaster.Identity)
        };
    }

    private async Task<NoAccessReason?> CalculateNoAccessReason(bool projectExists, Guid fusionProjectId)
    {
        if (!projectExists)
        {
            return NoAccessReason.ProjectDoesNotExist;
        }

        var (access, projectClassification) = await CalculateAccess(fusionProjectId);

        if (access.Contains(ActionType.Read))
        {
            return null;
        }

        return projectClassification switch
        {
            ProjectClassification.Internal => NoAccessReason.ClassificationInternal,
            ProjectClassification.Restricted => NoAccessReason.ClassificationRestricted,
            _ => NoAccessReason.ClassificationConfidential
        };
    }

    private async Task<(HashSet<ActionType>, ProjectClassification)> CalculateAccess(Guid fusionProjectId)
    {
        var projectId = await context.Projects
            .Where(p => !p.IsRevision && p.FusionProjectId == fusionProjectId)
            .Select(x => x.Id)
            .SingleAsync();

        var projectClassification = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectId)
            .Select(x => x.Classification)
            .SingleAsync();

        var projectMemberRole = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.UserId == currentUser.UserId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        var access = AccessCalculator.CalculateAccess(currentUser.ApplicationRoles, projectClassification, false, projectMemberRole);

        return (access, projectClassification);
    }
}
