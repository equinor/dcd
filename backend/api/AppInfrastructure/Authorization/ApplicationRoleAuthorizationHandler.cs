using System.Reflection;

using api.AppInfrastructure.Authorization.Extensions;
using api.AppInfrastructure.ControllerAttributes;
using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Models;

using Fusion;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Authorization;

public class ApplicationRoleAuthorizationHandler(IDbContextFactory<DcdDbContext> contextFactory, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<ApplicationRoleRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationRoleRequirement requirement)
    {
        var requestPath = httpContextAccessor.HttpContext?.Request.Path;

        if (requestPath?.StartsWithSegments(new PathString("/swagger")) == true)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (!await IsAuthorized(context, requirement))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private async Task<bool> IsAuthorized(AuthorizationHandlerContext context, ApplicationRoleRequirement roleRequirement)
    {
        var userRoles = context.User.DcdParseApplicationRoles();

        var userHasRequiredRole = userRoles.Any(roleRequirement.Roles.Contains);

        // TODO: Implement check for classification and project phase
        var project = await GetCurrentProject();

        if (project == null)
        {
            return userHasRequiredRole;
        }

        var actionType = GetActionTypeFromEndpoint();

        if (project.IsRevision && actionType == ActionType.Edit)
        {
            throw new ModifyRevisionException($"Cannot modify a revision. Project.Id={project.Id}");
        }

        var userId = context.User.GetAzureUniqueId()!.Value;
        var userMembershipRole = project.ProjectMembers.FirstOrDefault(pm => pm.UserId == userId)?.Role;

        List<ApplicationRole> requiredRolesForEdit = [ApplicationRole.Admin, ApplicationRole.User];
        List<ApplicationRole> requiredRolesForView = [ApplicationRole.Admin, ApplicationRole.User, ApplicationRole.ReadOnly];

        return actionType switch
        {
            ActionType.Edit => userRoles.Any(requiredRolesForEdit.Contains) || userMembershipRole == ProjectMemberRole.Editor,
            ActionType.Read => userRoles.Any(requiredRolesForView.Contains) || userMembershipRole == ProjectMemberRole.Editor || userMembershipRole == ProjectMemberRole.Observer,
            _ => userHasRequiredRole
        };
    }

    private ActionType? GetActionTypeFromEndpoint()
    {
        var endpoint = httpContextAccessor.HttpContext?.GetEndpoint();

        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor == null)
        {
            return null;
        }

        var actionTypeAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttribute<ActionTypeAttribute>();

        actionTypeAttribute ??= controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<ActionTypeAttribute>();

        return actionTypeAttribute?.ActionType;
    }

    private async Task<Project?> GetCurrentProject()
    {
        var projectId = httpContextAccessor.HttpContext?.Request.RouteValues["projectId"];
        if (projectId == null)
        {
            return null;
        }

        if (!Guid.TryParse(projectId.ToString(), out var projectIdGuid))
        {
            throw new InvalidProjectIdException($"Invalid project id: {projectId}");
        }

        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        var projectPk = await dbContext.GetPrimaryKeyForProjectIdOrRevisionId(projectIdGuid);

        return await dbContext.Projects.Include(p => p.ProjectMembers).SingleAsync(p => p.Id == projectPk);
    }
}
