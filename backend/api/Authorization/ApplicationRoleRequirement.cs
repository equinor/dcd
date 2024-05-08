using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public class ApplicationRoleRequirement : IAuthorizationRequirement
{
    public ApplicationRoleRequirement(List<ApplicationRole> roles)
    {
        Roles = roles;
    }

    public static ApplicationRole DefaultApplicationRole { get; } = ApplicationRole.User;

    public List<ApplicationRole> Roles { get; private set; }
}
