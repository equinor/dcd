using api.Models;

namespace api.Features.Projects.Update;

public static class ProjectClassificationHelper
{
    public static void AddCurrentUserAsEditorIfClassificationBecomesMoreStrict(
        Project existingProject,
        ProjectClassification newProjectClassification,
        Guid? currentUserId)
    {
        if (currentUserId == null)
        {
            return;
        }

        var classificationChangedToMoreStrict = ClassificationChangedToMoreStrict(existingProject, newProjectClassification);

        if (!classificationChangedToMoreStrict)
        {
            return;
        }

        if (existingProject.ProjectMembers.Any(x => x.UserId == currentUserId))
        {
            return;
        }

        existingProject.ProjectMembers.Add(new ProjectMember
        {
            UserId = currentUserId.Value,
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
