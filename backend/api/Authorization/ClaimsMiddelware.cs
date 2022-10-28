using System.Security.Claims;

using Fusion;
using Fusion.Integration.Authentication;
using Fusion.Integration.Profile;

namespace Api.Authorization;

public class ClaimsMiddelware
{

    public static string ApplicationRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private ILogger<ClaimsMiddelware> _logger;
    private readonly RequestDelegate nextMiddleware;
    public ClaimsMiddelware(RequestDelegate nextMiddleware,
        ILogger<ClaimsMiddelware> logger,
        IConfiguration configuration)
    {
        this.nextMiddleware = nextMiddleware;
        _logger = logger;

    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User == null)
        {
            _logger.LogError("User null");
        }
        var userId = httpContext.User?.GetAzureUniqueId();
        if (userId != null)
        {
            SetAppRoleClaims(httpContext);
        }
        else
        {
            _logger.LogError("Unauthenticated access attempted on: " + httpContext.Request.Path);
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
            _logger.LogInformation("Fusion Application Role: " + fusionApplicationRole.Value);
        }

        var applicationRoleClaims = applicationRoles
            .DefaultIfEmpty(ApplicationRole.None)
            .Select(role => new Claim(ApplicationRoleClaimType, role.ToString()));

        _logger.LogInformation("Application Roles: " + applicationRoleClaims);
        var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
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
            _logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.User;
        }

        if (httpContext.User.IsAccountType(FusionAccountType.External))
        {
            _logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.Admin;
        }

        if (httpContext.User.IsAccountType(FusionAccountType.Consultant))
        {
            _logger.LogInformation("Check for Fusion Account Type: " + ApplicationRole.User);
            return ApplicationRole.ReadOnly;
        }

        _logger.LogInformation("Check for Fusion Account Type: null");
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
