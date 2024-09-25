using api.Exceptions;
using api.Models.Interfaces;
using api.Repositories;

namespace api.Services;

public class ProjectAccessService : IProjectAccessService
{
    private readonly ILogger<ProjectAccessService> _logger;
    private readonly IProjectAccessRepository _projectAccessRepository;

    public ProjectAccessService(
        IProjectAccessRepository projectAccessRepository,
        ILoggerFactory loggerFactory
        )
    {
        _projectAccessRepository = projectAccessRepository;
        _logger = loggerFactory.CreateLogger<ProjectAccessService>();
    }

    /// <summary>
    /// Checks whether the project with the specified project ID exists on the entity with the given entity ID.
    /// This method is intended to be used to verify the project checked in the authorization handler is correct.
    /// </summary>
    /// <param name="projectIdFromUrl">The project ID extracted from the URL.</param>
    /// <param name="entityId">The ID of the entity being accessed.</param>
    public async Task ProjectExists<T>(Guid projectIdFromUrl, Guid entityId)
        where T : class, IHasProjectId
    {
        var entity = await _projectAccessRepository.Get<T>(entityId);
        if (entity == null)
        {
            throw new NotFoundInDBException($"Entity of type {typeof(T)} with id {entityId} not found.");
        }
        if (entity.ProjectId != projectIdFromUrl)
        {
            throw new ProjectAccessMismatchException($"Entity of type {typeof(T)} with id {entityId} does not belong to project with id {projectIdFromUrl}.", projectIdFromUrl, entityId);
        }
    }
}
