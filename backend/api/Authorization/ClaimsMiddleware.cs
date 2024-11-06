using System.Security.Claims;

using Fusion;
using Fusion.Integration.Authentication;
using Fusion.Integration.Profile;

namespace api.Authorization;

public class ClaimsMiddleware(
    RequestDelegate nextMiddleware,
    ILogger<ClaimsMiddleware> logger)
{
    public const string ApplicationRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

    public async Task InvokeAsync(HttpContext httpContext, CurrentUser currentUser)
    {
        if (httpContext.User == null)
        {
            logger.LogError("User null");
        }

        currentUser.Username = httpContext.User?.Identity?.Name;

        var userId = httpContext.User?.GetAzureUniqueId();
        if (userId != null)
        {
            SetAppRoleClaims(httpContext);
        }
        else
        {
            logger.LogError("Unauthenticated access attempted on: " + httpContext.Request.Path);
        }

        await nextMiddleware(httpContext);
    }

    private void SetAppRoleClaims(HttpContext httpContext)
    {
        var applicationRoles = new HashSet<ApplicationRole>();

        var applicationRolesFromAzure = RolesFromAzure(httpContext.User.Claims);
        applicationRoles.UnionWith(applicationRolesFromAzure);

        var fusionApplicationRole = RoleForAccountType(httpContext);
        if (fusionApplicationRole != null)
        {
            logger.LogInformation("Fusion Application Role: " + fusionApplicationRole.Value);
        }

        var applicationRoleClaims = applicationRoles
            .DefaultIfEmpty(ApplicationRole.None)
            .Select(role => new Claim(ApplicationRoleClaimType, role.ToString()));

        var rolesAsString = string.Join(",", applicationRoleClaims.Select(x => x.Value.ToString()));

        logger.LogInformation("Application Roles for User {UserName}: {roles}", httpContext.User?.Identity?.Name, rolesAsString);

        var claimsIdentity = httpContext.User?.Identity as ClaimsIdentity;
        if (claimsIdentity == null)
        {
            logger.LogError("ClaimsIdentity null");
            return;
        }
        claimsIdentity.AddClaims(applicationRoleClaims);
    }

    private ApplicationRole? RoleForAccountType(HttpContext httpContext)
    {
        // Check if we have Fusion claims available. If it is not present the Fusion extensions might throw exceptions on missing data.
        // This can happen when requests come in that do not have "Fusion-tokens" but general "Equinor tokens".
        var hasFusionIdentity = httpContext.User.Identities.Any(identity => identity is FusionIdentity);
        if (!hasFusionIdentity)
        {
            return null;
        }
        if (httpContext.User.IsAccountType(FusionAccountType.Employee))
        {
            logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.User;
        }

        if (httpContext.User.IsAccountType(FusionAccountType.External))
        {
            logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.Admin;
        }

        if (httpContext.User.IsAccountType(FusionAccountType.Consultant))
        {
            logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.ReadOnly;
        }

        logger.LogInformation("Check for Fusion Account Type: null");
        return null;
    }

    private IReadOnlySet<ApplicationRole> RolesFromAzure(IEnumerable<Claim> claims)
    {
        return claims
            .Where(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
            .Select(
                claim => Enum.TryParse(claim.Value, out ApplicationRole parsedRole) ? parsedRole : (ApplicationRole?)null
            )
            .Where(role => role != null)
            .Select(role => role!.Value)
            .ToHashSet();
    }
}
