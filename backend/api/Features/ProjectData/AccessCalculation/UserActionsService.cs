using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Features.ProjectData.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectData.AccessCalculation;

public class UserActionsService(CurrentUser currentUser, DcdDbContext context)
{
    public async Task<UserActionsDto> CalculateActions(Guid projectPk)
    {
        var projectClassification = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectPk)
            .Select(x => x.Classification)
            .SingleAsync();

        var projectMemberRole = await context.ProjectMembers
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.UserId == currentUser.UserId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        return Calculate(projectClassification, projectMemberRole);
    }

    public async Task<UserActionsDto> CalculateActions(Guid projectId, Guid revisionId)
    {
        var projectClassification = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.OriginalProjectId == projectId)
            .Where(x => x.Id == revisionId)
            .Select(x => x.Classification)
            .SingleAsync();

        var projectMemberRole = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.UserId == currentUser.UserId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        return Calculate(projectClassification, projectMemberRole);
    }

    private UserActionsDto Calculate(ProjectClassification projectClassification, ProjectMemberRole? projectMemberRole)
    {
        var accesses = AccessCalculator.CalculateAccess(currentUser.ApplicationRoles, projectClassification, false, projectMemberRole);

        return new UserActionsDto
        {
            CanView = accesses.Contains(ActionType.Read),
            CanEditProjectData = accesses.Contains(ActionType.Edit),
            CanCreateRevision = accesses.Contains(ActionType.CreateRevision),
            CanEditProjectMembers = accesses.Contains(ActionType.EditProjectMembers)
        };
    }
}

