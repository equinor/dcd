using api.Authorization;
using api.Authorization.Extensions;
using api.Dtos.Access;
using api.Exceptions;
using api.Models.Interfaces;
using api.Repositories;

namespace api.Services;

public class ProjectAccessService(
    IProjectAccessRepository projectAccessRepository,
    IHttpContextAccessor httpContextAccessor) : IProjectAccessService
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
        var entity = await projectAccessRepository.Get<T>(entityId);
        if (entity == null)
        {
            throw new NotFoundInDBException($"Entity of type {typeof(T)} with id {entityId} not found.");
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

        var _ = await projectAccessRepository.GetProjectByExternalId(externalId)
            ?? throw new NotFoundInDBException($"Project with external ID {externalId} not found.");

        bool isAdmin = userRoles.Contains(ApplicationRole.Admin);
        bool isUser = userRoles.Contains(ApplicationRole.User);
        bool isReadOnly = userRoles.Contains(ApplicationRole.ReadOnly);

        var accessRights = new AccessRightsDto
        {
            CanEdit = isAdmin || isUser,
            CanView = isAdmin || isUser || isReadOnly,
            IsAdmin = isAdmin
        };

        return accessRights;
    }
}
