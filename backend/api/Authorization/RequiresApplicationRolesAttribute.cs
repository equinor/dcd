using Microsoft.AspNetCore.Authorization;

namespace api.Authorization;

public sealed class RequiresApplicationRolesAttribute : AuthorizeAttribute
{
    public RequiresApplicationRolesAttribute(params ApplicationRole[] authorizedApplicationRoles)
    {
        AuthorizedApplicationRoles = authorizedApplicationRoles;
        Policy = ApplicationRolePolicyProvider.ApplicationRolesToPolicy(authorizedApplicationRoles);
    }

    public ApplicationRole[] AuthorizedApplicationRoles { get; }
}
