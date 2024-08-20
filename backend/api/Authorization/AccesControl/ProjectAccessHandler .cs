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
        var userGroups = GetUserGroups(context.User);

        var project = await GetCurrentProject(context);

        // bool isProjectMember = await CheckUserMembershipInProject(context.User, project);

        if (IsUserAuthorized(userGroups, project?.Classification, project?.ProjectPhase, true))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }

    private List<string> GetUserGroups(ClaimsPrincipal user)
    {
        var groupSids = user.Claims
                            .Where(c => c.Type == ClaimTypes.GroupSid)
                            .Select(c => c.Value)
                            .ToList();

        return groupSids;
    }

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

    private Task<bool> CheckUserMembershipInProject(ClaimsPrincipal user, Project? project)
    {
        return new Task<bool>(() => true);
    }

    private bool IsUserAuthorized(List<string> userGroups, ProjectClassification? classification, ProjectPhase? projectPhase, bool isProjectMember)
    {
        return true;
    }
}
