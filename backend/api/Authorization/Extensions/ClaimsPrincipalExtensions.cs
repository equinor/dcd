using System.Security.Claims;

namespace Api.Authorization.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns the all the assigned <see cref="ApplicationRole"/> of the current user (<see cref="ClaimsPrincipal"/>).
    /// </summary>
    public static List<ApplicationRole> AssignedApplicationRoles(this ClaimsPrincipal principal)
    {
        return principal.Claims
            .Where(claim => claim.Type == ClaimsMiddelware.ApplicationRoleClaimType)
            .Select(roleClaim => Enum.Parse<ApplicationRole>(roleClaim.Value))
            .ToList();
    }
}
