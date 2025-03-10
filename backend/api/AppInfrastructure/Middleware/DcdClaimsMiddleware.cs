using System.Security.Claims;

using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.Authorization.Extensions;

using Fusion;

namespace api.AppInfrastructure.Middleware;

public class DcdClaimsMiddleware(RequestDelegate nextMiddleware)
{
    private const string ApplicationRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

    public async Task InvokeAsync(HttpContext httpContext, CurrentUser currentUser)
    {
        if (httpContext.User.GetAzureUniqueId() == null || httpContext.User.GetAzureUniqueId() == null || httpContext.User.Identity is not ClaimsIdentity claimsIdentity)
        {
            await nextMiddleware(httpContext);

            return;
        }

        currentUser.Username = httpContext.User.Identity!.Name!;
        currentUser.AzureAdUserId = httpContext.User.GetAzureUniqueId()!.Value;
        currentUser.ApplicationRoles = httpContext.User.DcdParseApplicationRoles();

        claimsIdentity.AddClaims(httpContext.User.DcdParseApplicationRoles().Select(role => new Claim(ApplicationRoleClaimType, role.ToString())));

        await nextMiddleware(httpContext);
    }
}
