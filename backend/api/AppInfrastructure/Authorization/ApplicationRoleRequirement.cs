using Microsoft.AspNetCore.Authorization;

namespace api.AppInfrastructure.Authorization;

public class ApplicationRoleRequirement(List<ApplicationRole> roles) : IAuthorizationRequirement
{
    public static ApplicationRole DefaultApplicationRole => ApplicationRole.Admin;

    public List<ApplicationRole> Roles { get; private set; } = roles;
}
