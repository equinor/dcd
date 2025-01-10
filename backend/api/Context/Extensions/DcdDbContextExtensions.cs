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
}
