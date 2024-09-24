using System.Reflection;

using api.Authorization.Extensions;
using api.Controllers;
using api.Models;
using api.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;

namespace api.Authorization;

public class ApplicationRoleAuthorizationHandler : AuthorizationHandler<ApplicationRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ApplicationRoleAuthorizationHandler> _logger;



    public ApplicationRoleAuthorizationHandler(
        IProjectRepository projectRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ApplicationRoleAuthorizationHandler> logger,
        IMemoryCache cache
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _projectRepository = projectRepository;
        _cache = cache;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRoleRequirement requirement)
    {
        var requestPath = _httpContextAccessor.HttpContext?.Request.Path;

        // Accessing the swagger documentation is always allowed.
        if (IsAccessingSwagger(requestPath))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            HandleUnauthenticatedRequest(context, requestPath);
            return Task.CompletedTask;
        }

        var userRoles = context.User.AssignedApplicationRoles();
        if (!IsAuthorized(context, requirement, userRoles))
        {
            HandleUnauthorizedRequest(context, requestPath, requirement.Roles, userRoles);
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool IsAccessingSwagger(PathString? requestPath)
    {
        var swaggerPath = new PathString("/swagger");
        return requestPath?.StartsWithSegments(swaggerPath) == true;
    }

    private bool IsAuthorized(AuthorizationHandlerContext context, ApplicationRoleRequirement roleRequirement, List<ApplicationRole> userRoles)
    {
        var userHasRequiredRole = userRoles.Any(role => roleRequirement.Roles.Contains(role));

        var fusionIdentity = context.User.Identities.FirstOrDefault(i => i is Fusion.Integration.Authentication.FusionIdentity)
            as Fusion.Integration.Authentication.FusionIdentity;

        var azureUniqueId = fusionIdentity?.Profile?.AzureUniqueId ??
            throw new InvalidOperationException("AzureUniqueId not found in user profile");

        // TODO: Implement check for classification and project phase
        // var project = await GetCurrentProject(context);

        var actionType = GetActionTypeFromEndpoint();

        var requiredRolesForEdit = new List<ApplicationRole> { ApplicationRole.Admin, ApplicationRole.User };
        var requiredRolesForView = new List<ApplicationRole> { ApplicationRole.ReadOnly, ApplicationRole.Admin, ApplicationRole.User };

        if (actionType == ActionType.Edit)
        {
            userHasRequiredRole = userRoles.Any(role => requiredRolesForEdit.Contains(role));
        }
        else if (actionType == ActionType.Read)
        {
            userHasRequiredRole = userRoles.Any(role => requiredRolesForView.Contains(role));
        }

        return userHasRequiredRole;
    }

    private ActionType? GetActionTypeFromEndpoint()
    {
        var endpoint = _httpContextAccessor.HttpContext?.GetEndpoint();
        if (endpoint == null) { return null; }

        var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor == null) { return null; }

        var actionTypeAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttribute<ActionTypeAttribute>();

        actionTypeAttribute ??= controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<ActionTypeAttribute>();

        return actionTypeAttribute?.ActionType;
    }

    private async Task<Project?> GetCurrentProject(AuthorizationHandlerContext context)
    {
        var projectId = _httpContextAccessor.HttpContext?.Request.RouteValues["projectId"];
        if (projectId == null)
        {
            return null;
        }

        if (!Guid.TryParse(projectId.ToString(), out Guid projectIdGuid))
        {
            return null;
        }

        // Check if the project exists in the cache
        if (!_cache.TryGetValue(projectIdGuid, out Project? project))
        {
            // Get the project from the database
            project = await _projectRepository.GetProjectByIdOrExternalId(projectIdGuid);

            // Store the project in the cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(projectIdGuid, project, cacheEntryOptions);
        }

        return project;
    }

    private void HandleUnauthorizedRequest(
        AuthorizationHandlerContext context,
        PathString? requestPath,
        List<ApplicationRole> requiredAnyOfTheseRoles,
        List<ApplicationRole> userRoles
    )
    {
        context.Fail();
        var username = context.User.Identity!.Name;
        _logger.LogWarning(
            "User '{Username}' attempted to access '{RequestPath}' but was not authorized "
                + "- one of the following roles '{RequiredRoles}' is required , while user has the roles '{UserRoles}'",
            username,
            requestPath,
            string.Join(", ", requiredAnyOfTheseRoles),
            string.Join(", ", userRoles)
        );
    }

    private void HandleUnauthenticatedRequest(AuthorizationHandlerContext context, PathString? requestPath)
    {
        _logger.LogWarning("An unauthenticated user attempted to access '{RequestPath}'", requestPath);
        context.Fail();
    }
}
