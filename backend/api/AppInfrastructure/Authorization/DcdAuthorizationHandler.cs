using System.Reflection;
using System.Security.Claims;

using api.AppInfrastructure.Authorization.Extensions;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Context.Extensions;
using api.Features.ProjectAccess;
using api.Models.Enums;

using Fusion;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Authorization;

public class DcdAuthorizationHandler(IDbContextFactory<DcdDbContext> contextFactory, IHttpContextAccessor httpContextAccessor)
    : IAuthorizationHandler
{
    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var requestPath = httpContextAccessor.HttpContext?.Request.Path;

        if (requestPath?.StartsWithSegments(new PathString("/swagger")) == true)
        {
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();

            return;
        }

        if (!await IsAuthorized(context))
        {
            context.Fail();
        }
    }

    private async Task<bool> IsAuthorized(AuthorizationHandlerContext context)
    {
        var endpointAccessRequirement = GetActionTypeFromEndpoint();

        if (endpointAccessRequirement == null)
        {
            return true;
        }

        var userAccess = await GetUserAccess(context.User);

        return userAccess.Contains(endpointAccessRequirement.Value);
    }

    private async Task<HashSet<ActionType>> GetUserAccess(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetAzureUniqueId();
        var userRoles = claimsPrincipal.DcdParseApplicationRoles();

        if (userId == null)
        {
            return [];
        }

        return await GetUserAccessFromProjectOrRevision(userId.Value, userRoles);
    }

    private ActionType? GetActionTypeFromEndpoint()
    {
        var controllerActionDescriptor = httpContextAccessor.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();

        if (controllerActionDescriptor == null)
        {
            return null;
        }

        var attribute = controllerActionDescriptor.MethodInfo.GetCustomAttribute<AuthorizeActionTypeAttribute>()
                        ?? controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeActionTypeAttribute>();

        return attribute?.ActionType;
    }

    /// <summary>
    /// Checks if the revisionId is connected to the projectId and gets the originalProjectId of the revision
    /// </summary>
    /// <returns>originalProjectId of revision, or null if there is a mismatch</returns>
    private static async Task<Guid?> GetDataFromRevision(DcdDbContext dbContext, Guid revisionId, Guid? projectId)
    {
            var originalProjectId = await dbContext.Projects
                .Where(p => p.Id == revisionId && p.IsRevision)
                .Select(x => x.OriginalProjectId)
                .SingleAsync();

            if ((projectId != null && projectId != originalProjectId))
            {
                return null;
            }
            return originalProjectId;
    }

    private static async Task<(ProjectMemberRole? ProjectMemberAccess, ProjectClassification Classification, bool isRevision) > GetDataFromProject(DcdDbContext dbContext, Guid projectPk, Guid userId)
    {
        var data = await dbContext.Projects
            .Where(p => p.Id == projectPk)
            .Select(x => new
            {
                ProjectIdWithProjectMembersConnected = x.OriginalProjectId ?? x.Id,
                Classification = x.IsRevision ? x.OriginalProject!.Classification : x.Classification,
                x.IsRevision
            })
            .SingleAsync();

        var projectMemberAccess = await dbContext.ProjectMembers
            .Where(x => x.ProjectId == data.ProjectIdWithProjectMembersConnected)
            .Where(x => x.UserId == userId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        return (projectMemberAccess, data.Classification, data.IsRevision);
    }

    private async Task<HashSet<ActionType>> GetUserAccessFromProjectOrRevision(Guid userId, HashSet<ApplicationRole> userRoles)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var projectPk = await ResolveProjectPkFromRoute(dbContext);
        var revisionId = GetIdFromRoute("revisionId");

        if (revisionId != null)
        {
            projectPk = await GetDataFromRevision(dbContext, revisionId.Value, projectPk);

            if (projectPk == null)
            {
                return [];
            }

        }
        else if (projectPk == null)
        {
            return [];
        }

        var (projectMemberRole, projectClassification, projectIsRevision) = await GetDataFromProject(dbContext, projectPk.Value, userId);

        var isRevision = revisionId != null || projectIsRevision;
        return AccessCalculator.CalculateAccess(userRoles, projectClassification, isRevision, projectMemberRole);
    }

    private async Task<Guid?> ResolveProjectPkFromRoute(DcdDbContext dbContext)
    {
        var projectIdGuid = GetIdFromRoute("projectId");

        if (projectIdGuid != null)
        {
            return await dbContext.GetPrimaryKeyForProjectIdOrRevisionId(projectIdGuid.Value);
        }
        return null;
    }

    private Guid? GetIdFromRoute(string routeId)
    {
        var revisionId = httpContextAccessor.HttpContext?.Request.RouteValues[routeId];
        return revisionId != null ? Guid.Parse(revisionId.ToString()!) : null;
    }
}
