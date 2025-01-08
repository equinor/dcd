using System.Security.Claims;

using Fusion;
using Fusion.Integration.Authentication;
using Fusion.Integration.Profile;

namespace api.AppInfrastructure.Authorization.Extensions;

public static class ClaimsExtensions
{
    public static HashSet<ApplicationRole> DcdParseApplicationRoles(this ClaimsPrincipal principal)
    {
        var applicationRoles = RolesFromAzure(principal.Claims);

        var fusionApplicationRole = RoleForAccountType(principal);

        if (fusionApplicationRole != null)
        {
            applicationRoles.Add(fusionApplicationRole.Value);
        }

        return applicationRoles;
    }

    private static HashSet<ApplicationRole> RolesFromAzure(IEnumerable<Claim> claims)
    {
        return claims
            .Where(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
            .Select(claim => Enum.TryParse(claim.Value, out ApplicationRole parsedRole) ? parsedRole : (ApplicationRole?)null)
            .Where(role => role != null && role != ApplicationRole.None)
            .Select(role => role!.Value)
            .ToHashSet();
    }

    private static ApplicationRole? RoleForAccountType(ClaimsPrincipal principal)
    {
        // Check if we have Fusion claims available. If it is not present the Fusion extensions might throw exceptions on missing data.
        // This can happen when requests come in that do not have "Fusion-tokens" but general "Equinor tokens".
        var hasFusionIdentity = principal.Identities.Any(identity => identity is FusionIdentity);

        if (!hasFusionIdentity)
        {
            return null;
        }

        if (principal.IsAccountType(FusionAccountType.External))
        {
            return ApplicationRole.Admin;
        }

        if (principal.IsAccountType(FusionAccountType.Employee))
        {
            return ApplicationRole.User;
        }

        if (principal.IsAccountType(FusionAccountType.Consultant))
        {
            return ApplicationRole.ReadOnly;
        }

        return null;
    }
}
