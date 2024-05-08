using System.Security.Claims;

namespace api.Authorization.Extensions;

public static class ClaimsPrincipalExtensions
{
    public const string AzureClaimIssuer = "https://sts.windows.net/3aa4a235-b6e2-48d5-9195-7fcf05b459b0/";
    /// <summary>
    /// Returns the all the assigned <see cref="ApplicationRole"/> of the current user (<see cref="ClaimsPrincipal"/>).
    /// </summary>
    public static List<ApplicationRole> AssignedApplicationRoles(this ClaimsPrincipal principal)
    {
        return principal.Claims
            .Where(claim => claim.Type == ClaimsMiddelware.ApplicationRoleClaimType && claim.Issuer == AzureClaimIssuer)
            .Select(roleClaim => Enum.Parse<ApplicationRole>(roleClaim.Value))
            .ToList();
    }
}
