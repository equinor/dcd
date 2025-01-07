using api.AppInfrastructure.Authorization;
using api.Context;
using api.Features.ProjectData.Dtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectAccess.V2;

public class ProjectAccessServiceV2(DcdDbContext context, CurrentUser currentUser)
{
    public async Task<bool> UserHasViewAccessToProject(Guid projectPk)
    {
        var projectClassification = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectPk)
            .Select(x => x.Classification)
            .SingleAsync();

        var userIsConnectedToProject = await context.ProjectMembers
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.UserId == currentUser.UserId)
            .AnyAsync();

        var accesses = AccessCalculator.CalculateAccess(currentUser, projectClassification, false, userIsConnectedToProject);

        return accesses.Contains(AccessActions.View);
    }

    public async Task<bool> UserHasViewAccessToRevision(Guid projectId, Guid revisionId)
    {
        var projectClassification = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.OriginalProjectId == projectId)
            .Where(x => x.Id == revisionId)
            .Select(x => x.Classification)
            .SingleAsync();

        var userIsConnectedToProject = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.UserId == currentUser.UserId)
            .AnyAsync();

        var accesses = AccessCalculator.CalculateAccess(currentUser, projectClassification, true, userIsConnectedToProject);

        return accesses.Contains(AccessActions.View);
    }

    public async Task<ActionsDto> GetAccess(Guid projectPk)
    {
        var projectClassification = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectPk)
            .Select(x => x.Classification)
            .SingleAsync();

        var userIsConnectedToProject = await context.ProjectMembers
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.UserId == currentUser.UserId)
            .AnyAsync();

        var accesses = AccessCalculator.CalculateAccess(currentUser, projectClassification, false, userIsConnectedToProject);

        return new ActionsDto
        {
            CanView = accesses.Contains(AccessActions.View),
            CanCreateRevision = accesses.Contains(AccessActions.CreateRevision)
        };
    }

    public async Task<ActionsDto> GetAccess(Guid projectId, Guid revisionId)
    {
        var projectClassification = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.OriginalProjectId == projectId)
            .Where(x => x.Id == revisionId)
            .Select(x => x.Classification)
            .SingleAsync();

        var userIsConnectedToProject = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.UserId == currentUser.UserId)
            .AnyAsync();

        var accesses = AccessCalculator.CalculateAccess(currentUser, projectClassification, true, userIsConnectedToProject);

        return new ActionsDto
        {
            CanView = accesses.Contains(AccessActions.View),
            CanCreateRevision = accesses.Contains(AccessActions.CreateRevision)
        };
    }
}
