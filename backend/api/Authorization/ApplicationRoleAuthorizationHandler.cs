namespace Api.Authorization;

using System.Threading.Tasks;

using Api.Authorization.Extensions;

using Microsoft.AspNetCore.Authorization;

public class ApplicationRoleAuthorizationHandler : AuthorizationHandler<ApplicationRoleRequirement>
{
    private readonly IHttpContextAccessor httpContextAccessor;


    public ApplicationRoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRoleRequirement requirement)
    {
        var requestPath = httpContextAccessor.HttpContext?.Request.Path;

        if (context.User.Identity?.IsAuthenticated != true)
        {
            HandleUnauthenticatedRequest(context, requestPath);
            return Task.CompletedTask;
        }

        var userRoles = context.User.AssignedApplicationRoles();
        if (!IsAuthorized(requirement, userRoles))
        {
            Console.WriteLine(context.User.AssignedApplicationRoles());
            HandleUnauthorizedRequest(context, requestPath, requirement.Roles, userRoles);
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool IsAuthorized(ApplicationRoleRequirement roleRequirement, List<ApplicationRole> userRoles)
    {
        return userRoles.Any(role => roleRequirement.Roles.Contains(role));
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
    }

    private void HandleUnauthenticatedRequest(AuthorizationHandlerContext context, PathString? requestPath)
    {
        context.Fail();
    }
}
