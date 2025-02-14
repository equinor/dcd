using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectAccess;

public class UserActionsService(CurrentUser currentUser, DcdDbContext context)
{
    public async Task<UserActionsDto> CalculateActionsForProject(Guid projectPk)
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

        return Calculate(projectClassification, projectMemberRole, false);
    }

    public async Task<UserActionsDto> CalculateActionsForRevision(Guid primaryKeyValue)
    {
        var revisionData = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.Id == primaryKeyValue)
            .Select(x => new
            {
                x.OriginalProject!.Classification,
                OriginalProjectId = x.OriginalProjectId!.Value
            })
            .SingleAsync();

        var projectMemberRole = await context.ProjectMembers
            .Where(x => x.Id == revisionData.OriginalProjectId)
            .Where(x => x.UserId == currentUser.UserId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        return Calculate(revisionData.Classification, projectMemberRole, true);
    }

    private UserActionsDto Calculate(ProjectClassification projectClassification, ProjectMemberRole? projectMemberRole, bool isRevision)
    {
        var accesses = AccessCalculator.CalculateAccess(currentUser.ApplicationRoles, projectClassification, isRevision, projectMemberRole);

        return new UserActionsDto
        {
            CanView = accesses.Contains(ActionType.Read),
            CanEditProjectData = accesses.Contains(ActionType.Edit),
            CanCreateRevision = accesses.Contains(ActionType.CreateRevision),
            CanEditProjectMembers = accesses.Contains(ActionType.EditProjectMembers)
        };
    }
}

