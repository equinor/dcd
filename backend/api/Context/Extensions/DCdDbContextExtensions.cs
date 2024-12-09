using api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Extensions;

public static class DCdDbContextExtensions
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

        throw new NotFoundInDBException($"Could not find project with id {projectId}.");
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

        throw new NotFoundInDBException($"Project id {projectId} not found in db for project or revision.");
    }

    public static async Task<Guid> GetPrimaryKeyForRevisionId(this DcdDbContext context, Guid revisionId)
    {
        var matchingPrimaryKeysForRevisions = await context.Projects
            .Where(x => x.IsRevision)
            .Where(x => x.Id == revisionId)
            .Select(x => x.Id)
            .ToListAsync();

        if (matchingPrimaryKeysForRevisions.Count == 1)
        {
            return matchingPrimaryKeysForRevisions.Single();
        }

        throw new NotFoundInDBException($"Could not find revision with id {revisionId}.");
    }
}
