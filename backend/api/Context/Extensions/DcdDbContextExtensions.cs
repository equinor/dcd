using api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Extensions;

public static class DcdDbContextExtensions
{
    public static async Task<Guid> GetPrimaryKeyForProjectId(this DcdDbContext context, Guid projectId)
    {
        var matchingPrimaryKeys = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectId || x.FusionProjectId == projectId)
            .Select(x => x.Id)
            .ToListAsync();

        if (matchingPrimaryKeys.Count == 1)
        {
            return matchingPrimaryKeys.Single();
        }

        if (matchingPrimaryKeys.Count > 1)
        {
            throw new Exception($"Found more than one matching project for project id {projectId}.");
        }

        throw new NotFoundInDbException($"Could not find project with id {projectId}.");
    }

    public static async Task EnsureRevisionIsConnectedToProject(this DcdDbContext context, Guid projectId, Guid revisionId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var originalProjectIdForRevision = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.Id == revisionId)
            .Select(x => x.OriginalProjectId!.Value)
            .SingleOrDefaultAsync();

        if (originalProjectIdForRevision == projectPk)
        {
            return;
        }

        throw new NotFoundInDbException($"RevisionId {revisionId} is not connected to ProjectId {projectId}.");
    }

    public static async Task<Guid> GetPrimaryKeyForProjectIdOrRevisionId(this DcdDbContext context, Guid projectId)
    {
        var matchingPrimaryKeys = await context.Projects
            .Where(x => !x.IsRevision)
            .Where(x => x.Id == projectId || x.FusionProjectId == projectId)
            .Select(x => x.Id)
            .ToListAsync();

        if (matchingPrimaryKeys.Count == 1)
        {
            return matchingPrimaryKeys.Single();
        }

        if (matchingPrimaryKeys.Count > 1)
        {
            throw new Exception($"Found more than one matching project for project id {projectId}.");
        }

        var matchingPrimaryKeysForRevisions = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.Id == projectId)
            .Select(x => x.Id)
            .ToListAsync();

        if (matchingPrimaryKeysForRevisions.Count == 1)
        {
            return matchingPrimaryKeysForRevisions.Single();
        }

        throw new NotFoundInDbException($"Project id {projectId} not found in db for project or revision.");
    }

    public static async Task UpdateCaseUpdatedUtc(this DcdDbContext context, Guid caseId)
    {
        var caseItem = await context.Cases.SingleAsync(c => c.Id == caseId);

        caseItem.UpdatedUtc = DateTime.UtcNow;
    }

    public static async Task UpdateProjectUpdatedUtc(this DcdDbContext context, Guid projectPk)
    {
        var project = await context.Projects.SingleAsync(c => c.Id == projectPk);

        project.UpdatedUtc = DateTime.UtcNow;
    }
}
