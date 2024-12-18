using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.Authorization.Extensions;
using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Models.Interfaces;

namespace api.Features.ProjectAccess;

public class ProjectAccessService(DcdDbContext context, IHttpContextAccessor httpContextAccessor) : IProjectAccessService
{
    /// <summary>
    /// Checks whether the project with the specified project ID exists on the entity with the given entity ID.
    /// This method is intended to be used to verify the project checked in the authorization handler is correct.
    /// </summary>
    /// <param name="projectIdFromUrl">The project ID extracted from the URL.</param>
    /// <param name="entityId">The ID of the entity being accessed.</param>
    public async Task ProjectExists<T>(Guid projectIdFromUrl, Guid entityId)
        where T : class, IHasProjectId
    {
        var entity = await context.Set<T>().FindAsync(entityId);

        if (entity == null)
        {
            throw new NotFoundInDbException($"Entity of type {typeof(T)} with id {entityId} not found.");
        }

        if (entity.ProjectId != projectIdFromUrl)
        {
            throw new ProjectAccessMismatchException($"Entity of type {typeof(T)} with id {entityId} does not belong to project with id {projectIdFromUrl}.", projectIdFromUrl, entityId);
        }
    }

    public async Task<AccessRightsDto> GetUserProjectAccess(Guid externalId)
    {
        var userRoles = httpContextAccessor.HttpContext?.User.AssignedApplicationRoles();

        if (userRoles == null)
        {
            return new AccessRightsDto
            {
                CanEdit = false,
                CanView = false,
                IsAdmin = false
            };
        }

        await context.GetPrimaryKeyForProjectId(externalId);

        var isAdmin = userRoles.Contains(ApplicationRole.Admin);
        var isUser = userRoles.Contains(ApplicationRole.User);
        var isReadOnly = userRoles.Contains(ApplicationRole.ReadOnly);

        return new AccessRightsDto
        {
            CanEdit = isAdmin || isUser,
            CanView = isAdmin || isUser || isReadOnly,
            IsAdmin = isAdmin
        };
    }
}
