using System.Reflection;

using api.AppInfrastructure.Authorization.Extensions;
using api.Context;
using api.Controllers;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Authorization;

public class ApplicationRoleAuthorizationHandler(
    DcdDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    ILogger<ApplicationRoleAuthorizationHandler> logger)
    : AuthorizationHandler<ApplicationRoleRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ApplicationRoleRequirement requirement
    )
    {
        var requestPath = httpContextAccessor.HttpContext?.Request.Path;

        // Accessing the swagger documentation is always allowed.
        if (IsAccessingSwagger(requestPath))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            HandleUnauthenticatedRequest(context, requestPath);
            return Task.CompletedTask;
        }

        var userRoles = context.User.AssignedApplicationRoles();
        if (!await IsAuthorized(context, requirement, userRoles))
        {
            HandleUnauthorizedRequest(context, requestPath, requirement.Roles, userRoles);
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool IsAccessingSwagger(PathString? requestPath)
    {
        var swaggerPath = new PathString("/swagger");
        return requestPath?.StartsWithSegments(swaggerPath) == true;
    }

    private static Guid GetAzureUniqueId(AuthorizationHandlerContext context)
    {
        var fusionIdentity = context.User.Identities.FirstOrDefault(i => i is Fusion.Integration.Authentication.FusionIdentity)
            as Fusion.Integration.Authentication.FusionIdentity;

        return fusionIdentity?.Profile?.AzureUniqueId ??
            throw new InvalidOperationException("AzureUniqueId not found in user profile");
    }

    private async Task<bool> IsAuthorized(
        AuthorizationHandlerContext context,
        ApplicationRoleRequirement roleRequirement,
        List<ApplicationRole> userRoles
    )
    {
        var userHasRequiredRole = userRoles.Any(roleRequirement.Roles.Contains);

        // TODO: Implement check for classification and project phase
        var project = await GetCurrentProject(context);

        if (project == null)
        {
            return userHasRequiredRole;
        }

        var actionType = GetActionTypeFromEndpoint();

        if (project.IsRevision && actionType == ActionType.Edit)
        {
            throw new ModifyRevisionException("Cannot modify a revision", project.Id);
        }

        var azureUniqueId = GetAzureUniqueId(context);
        var userMembershipRole = GetUserMembershipRole(project, azureUniqueId);

        List<ApplicationRole> requiredRolesForEdit = [ApplicationRole.Admin, ApplicationRole.User];
        List<ApplicationRole> requiredRolesForView = [ApplicationRole.Admin, ApplicationRole.User, ApplicationRole.ReadOnly];

        return actionType switch
        {
            ActionType.Edit => userRoles.Any(requiredRolesForEdit.Contains) || userMembershipRole == ProjectMemberRole.Editor,
            ActionType.Read => userRoles.Any(requiredRolesForView.Contains) || userMembershipRole == ProjectMemberRole.Editor || userMembershipRole == ProjectMemberRole.Observer,
            _ => userHasRequiredRole
        };
    }

    private static ProjectMemberRole? GetUserMembershipRole(Project project, Guid azureUniqueId)
    {
        var projectMember = project.ProjectMembers.FirstOrDefault(pm => pm.UserId == azureUniqueId);
        return projectMember?.Role;
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

    private async Task<Project?> GetCurrentProject(AuthorizationHandlerContext context)
    {
        var projectId = httpContextAccessor.HttpContext?.Request.RouteValues["projectId"];
        if (projectId == null)
        {
            return null;
        }

        if (!Guid.TryParse(projectId.ToString(), out var projectIdGuid))
        {
            // TODO: Should this throw an error?
            return null;
        }

        var project = await dbContext.Projects
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == projectIdGuid || (p.FusionProjectId == projectIdGuid && !p.IsRevision));

        // /*
        // Some projects have the external id set as the id.
        // This may cause updates to projects where the external id is the same as the project id
        // to return a revision with the same external id instead.
        // Updates to revsions are not allowed and an error is thrown.
        // Therefore, we split the database call into two separate calls, first looking for the project by project id.
        // */
        // if (project == null)
        // {
        //     project = await _projectAccessRepository.GetProjectByExternalId(projectIdGuid);
        // }

        // TODO: If no project is found, should this throw an error?

        return project;
    }

    private void HandleUnauthorizedRequest(
        AuthorizationHandlerContext context,
        PathString? requestPath,
        List<ApplicationRole> requiredAnyOfTheseRoles,
        List<ApplicationRole> userRoles
    )
    {
        context.Fail();
        var username = context.User.Identity!.Name;
        logger.LogWarning(
            "User '{Username}' attempted to access '{RequestPath}' but was not authorized "
                + "- one of the following roles '{RequiredRoles}' is required , while user has the roles '{UserRoles}'",
            username,
            requestPath,
            string.Join(", ", requiredAnyOfTheseRoles),
            string.Join(", ", userRoles)
        );
    }

    private void HandleUnauthenticatedRequest(AuthorizationHandlerContext context, PathString? requestPath)
    {
        logger.LogWarning("An unauthenticated user attempted to access '{RequestPath}'", requestPath);
        context.Fail();
    }
}
