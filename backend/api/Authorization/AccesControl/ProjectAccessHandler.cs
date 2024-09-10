using System.Security.Claims;

using api.Models;
using api.Repositories;
using api.Services;

using Microsoft.AspNetCore.Authorization;

public class ProjectAccessHandler : AuthorizationHandler<ProjectAccessRequirement>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFusionPeopleService _fusionPeopleService;


    public ProjectAccessHandler(
        IProjectRepository projectRepository,
        IHttpContextAccessor httpContextAccessor,
        IFusionPeopleService fusionPeopleService
    )
    {
        _projectRepository = projectRepository;
        _httpContextAccessor = httpContextAccessor;
        _fusionPeopleService = fusionPeopleService;
    }

    protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAccessRequirement requirement)
    {
        // var userGroups = GetUserGroups(context.User);

        var project = await GetCurrentProject(context);

        bool isProjectMember = CheckUserMembershipInProject(context.User, project);

        if (IsUserAuthorized(project?.Classification, project?.ProjectPhase, isProjectMember))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }

    // private List<string> GetUserGroups(ClaimsPrincipal user)
    // {
    //     var groupSids = user.Claims
    //                         .Where(c => c.Type == ClaimTypes.GroupSid)
    //                         .Select(c => c.Value)
    //                         .ToList();

    //     return groupSids;
    // }

    private async Task<Project?> GetCurrentProject(AuthorizationHandlerContext context)
    {
        var projectId = _httpContextAccessor.HttpContext?.Request.RouteValues["projectId"];
        if (projectId == null)
        {
            return null;
        }
        _ = Guid.TryParse(projectId.ToString(), out Guid projectIdGuid);
        var project = await _projectRepository.GetProjectByIdOrExternalId(projectIdGuid);
        return project;
    }

    private bool CheckUserMembershipInProject(ClaimsPrincipal user, Project? project)
    {
        var fusionIdentity = user.Identities.FirstOrDefault(i => i is Fusion.Integration.Authentication.FusionIdentity)
            as Fusion.Integration.Authentication.FusionIdentity;

        var azureUniqueId = fusionIdentity?.Profile?.AzureUniqueId ??
            throw new InvalidOperationException("AzureUniqueId not found in user profile");
        Console.WriteLine("azureUniqueId: " + azureUniqueId);
        if (project == null)
        {
            return false;
        }
        var projectMembers = project.ProjectMembers;
        if (projectMembers == null)
        {
            return false;
        }
        var result = projectMembers.Any(pm => pm.AzureUniqueId == azureUniqueId);
        return result;
    }

    private bool IsUserAuthorized(ProjectClassification? classification, ProjectPhase? projectPhase, bool isProjectMember)
    {
        if (classification == ProjectClassification.Confidential || classification == ProjectClassification.Restricted)
        {
            if (!isProjectMember)
            {
                return false;
            }
        }
        return true;
    }
}
