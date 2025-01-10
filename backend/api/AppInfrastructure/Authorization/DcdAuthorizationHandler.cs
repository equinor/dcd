using System.Reflection;
using System.Security.Claims;

using api.AppInfrastructure.Authorization.Extensions;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.ProjectData.AccessCalculation;
using api.Models;

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

        var projectId = httpContextAccessor.HttpContext?.Request.RouteValues["projectId"];

        if (projectId == null)
        {
            return [];
        }

        if (!Guid.TryParse(projectId.ToString(), out var projectIdGuid))
        {
            throw new InvalidInputException($"Invalid project id: {projectId}");
        }

        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        var projectPk = await dbContext.GetPrimaryKeyForProjectIdOrRevisionId(projectIdGuid);

        var data = await dbContext.Projects
            .Where(p => p.Id == projectPk)
            .Select(x => new
            {
                ProjectIdWithProjectMembersConnected = x.OriginalProjectId ?? x.Id,
                x.Classification,
                x.IsRevision
            })
            .SingleAsync();

        var projectMemberAccess = await dbContext.ProjectMembers
            .Where(x => x.ProjectId == data.ProjectIdWithProjectMembersConnected)
            .Where(x => x.UserId == userId)
            .Select(x => (ProjectMemberRole?)x.Role)
            .SingleOrDefaultAsync();

        return AccessCalculator.CalculateAccess(userRoles, data.Classification, data.IsRevision, projectMemberAccess);
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
}
