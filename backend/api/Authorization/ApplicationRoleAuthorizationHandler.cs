using Microsoft.AspNetCore.Authorization;

using api.Authorization.Extensions;

namespace api.Authorization;

public class ApplicationRoleAuthorizationHandler : AuthorizationHandler<ApplicationRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public ApplicationRoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRoleRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            HandleUnauthenticatedRequest(context);
            return Task.CompletedTask;
        }

        var userRoles = context.User.AssignedApplicationRoles();
        if (!IsAuthorized(requirement, userRoles))
        {
            Console.WriteLine(context.User.AssignedApplicationRoles());
            HandleUnauthorizedRequest(context);
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool IsAuthorized(ApplicationRoleRequirement roleRequirement, List<ApplicationRole> userRoles)
    {
        return userRoles.Any(role => roleRequirement.Roles.Contains(role));
    }

    private static void HandleUnauthorizedRequest(AuthorizationHandlerContext context)
    {
        context.Fail();
    }

    private static void HandleUnauthenticatedRequest(AuthorizationHandlerContext context)
    {
        context.Fail();
    }
}
