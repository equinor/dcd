using System.Security.Claims;

using Fusion;
using Fusion.Integration.Authentication;
using Fusion.Integration.Profile;

namespace Api.Authorization;

public class ClaimsMiddelware
{

    public const string ApplicationRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private readonly RequestDelegate nextMiddleware;
    public ClaimsMiddelware(RequestDelegate nextMiddleware,        
        ILogger<ClaimsMiddelware> logger,
        IConfiguration configuration) {
                var InvestmentArenaOrgChartId = new Guid();
                this.nextMiddleware = nextMiddleware;

    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if(httpContext.User == null) {
            Console.WriteLine("User null");
        }
        var userId = httpContext.User.GetAzureUniqueId();
        if (userId != null)
        {
            SetAppRoleClaims(httpContext);

        }
        else
        {
            Console.WriteLine("Unauthenticated access attempted on: " +httpContext.Request.Path);
            // logger.LogInformation("Unauthenticated access attempted on '{Path}'", httpContext.Request.Path);
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
            Console.WriteLine(fusionApplicationRole.Value);

        var applicationRoleClaims = applicationRoles
            .DefaultIfEmpty(ApplicationRole.None)
            .Select(role => new Claim(ApplicationRoleClaimType, role.ToString()));
        
        Console.WriteLine(applicationRoleClaims);
        var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
        claimsIdentity.AddClaims(applicationRoleClaims);
    }

    private ApplicationRole? RoleForAccountType(HttpContext httpContext)
    {
        // Check if we have Fusion claims available. If it is not present the Fusion extensions might throw exceptions on missing data.
        // This can happen when requests come in that do not have "Fusion-tokens" but general "Equinor tokens".
        var hasFusionIdentity = httpContext.User.Identities.Any(identity => identity is FusionIdentity);
        if (!hasFusionIdentity)
            return null;

        if (httpContext.User.IsAccountType(FusionAccountType.Employee))
            return ApplicationRole.User;

        if (httpContext.User.IsAccountType(FusionAccountType.External))
            return ApplicationRole.Admin;

        if (httpContext.User.IsAccountType(FusionAccountType.Consultant))
            return ApplicationRole.ReadOnly;

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
