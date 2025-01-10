using api.Context;
using api.Exceptions;
using api.Models.Interfaces;

namespace api.Features.ProjectIntegrity;

public class ProjectIntegrityService(DcdDbContext context) : IProjectIntegrityService
{
    /// <summary>
    /// Checks whether the project with the specified project ID exists on the entity with the given entity ID.
    /// This method is intended to be used to verify the project checked in the authorization handler is correct.
    /// </summary>
    /// <param name="projectIdFromUrl">The project ID extracted from the URL.</param>
    /// <param name="entityId">The ID of the entity being accessed.</param>
    public async Task EntityIsConnectedToProject<T>(Guid projectIdFromUrl, Guid entityId)
        where T : class, IHasProjectId
    {
        var entity = await context.Set<T>().FindAsync(entityId);

        if (entity == null)
        {
            throw new NotFoundInDbException($"Entity of type {typeof(T)} with id {entityId} not found.");
        }

        if (entity.ProjectId != projectIdFromUrl)
        {
            throw new ProjectAccessMismatchException($"Entity of type {typeof(T)} with id {entityId} does not belong to project with id {projectIdFromUrl}.");
        }
    }
}
