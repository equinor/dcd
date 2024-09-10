using api.Authorization.Extensions;

using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public class ApplicationRoleAuthorizationHandler : AuthorizationHandler<ApplicationRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ApplicationRoleAuthorizationHandler> _logger;



    public ApplicationRoleAuthorizationHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<ApplicationRoleAuthorizationHandler> logger
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
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

    private static bool IsAuthorized(AuthorizationHandlerContext context, ApplicationRoleRequirement roleRequirement, List<ApplicationRole> userRoles)
    {
        var userHasRequiredRole = userRoles.Any(role => roleRequirement.Roles.Contains(role));

                var fusionIdentity = context.User.Identities.FirstOrDefault(i => i is Fusion.Integration.Authentication.FusionIdentity)
            as Fusion.Integration.Authentication.FusionIdentity;

        var azureUniqueId = fusionIdentity?.Profile?.AzureUniqueId ??
            throw new InvalidOperationException("AzureUniqueId not found in user profile");
        Console.WriteLine("azureUniqueId: " + azureUniqueId);

        return userHasRequiredRole;
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
