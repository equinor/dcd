using api.AppInfrastructure.Authorization;
using api.Models;

namespace api.Features.Projects.Update;

public static class ProjectClassificationHelper
{
    public static void AddCurrentUserAsEditorIfClassificationBecomesMoreStrict(
        Project existingProject,
        ProjectClassification newProjectClassification,
        CurrentUser? currentUser)
    {
        if (currentUser == null)
        {
            return;
        }

        if (currentUser.ApplicationRoles.Contains(ApplicationRole.Admin) ||
            currentUser.ApplicationRoles.Contains(ApplicationRole.ReadOnly))
        {
            return;
        }

        var classificationChangedToMoreStrict = ClassificationChangedToMoreStrict(existingProject, newProjectClassification);

        if (!classificationChangedToMoreStrict)
        {
            return;
        }

        if (existingProject.ProjectMembers.Any(x => x.UserId == currentUser.UserId))
        {
            return;
        }

        existingProject.ProjectMembers.Add(new ProjectMember
        {
            UserId = currentUser.UserId,
            ProjectId = existingProject.Id,
            Role = ProjectMemberRole.Editor,
            FromOrgChart = false
        });
    }

    public static bool ClassificationChangedToMoreStrict(Project existingProject, ProjectClassification newProjectClassification)
    {
        var rolesRequiringExplicitAccess = new List<ProjectClassification>
        {
            ProjectClassification.Confidential,
            ProjectClassification.Restricted
        };

        if (rolesRequiringExplicitAccess.Contains(existingProject.Classification))
        {
            return false;
        }

        return rolesRequiringExplicitAccess.Contains(newProjectClassification);
    }
}
