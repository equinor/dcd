using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Models.Enums;

namespace api.Features.ProjectAccess;

public static class AccessCalculator
{
    public static HashSet<ActionType> CalculateAccess(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, bool isRevision, ProjectMemberRole? projectMemberRole)
    {
        var actions = new HashSet<ActionType>();

        if (CanView(userRoles, projectClassification, projectMemberRole))
        {
            actions.Add(ActionType.Read);
        }

        if (CanCreateRevision(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(ActionType.CreateRevision);
        }

        if (CanEditProjectData(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(ActionType.Edit);
        }

        if (CanEditProjectMembers(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(ActionType.EditProjectMembers);
        }

        return actions;
    }

    private static bool CanView(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, ProjectMemberRole? projectMemberRole)
    {
        if (projectMemberRole != null)
        {
            return true;
        }

        if (userRoles.Count == 0)
        {
            return false;
        }

        if (userRoles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        return projectClassification is ProjectClassification.Open or ProjectClassification.Internal;
    }

    private static bool CanCreateRevision(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, bool isRevision, ProjectMemberRole? projectMemberRole)
    {
        if (isRevision)
        {
            return false;
        }

        if (userRoles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (projectMemberRole == ProjectMemberRole.Editor)
        {
            return true;
        }

        if (!userRoles.Contains(ApplicationRole.User))
        {
            return false;
        }

        if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
        {
            return true;
        }

        return false;
    }

    private static bool CanEditProjectData(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, bool isRevision, ProjectMemberRole? projectMemberRole)
    {
        if (isRevision)
        {
            return false;
        }

        if (userRoles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (projectMemberRole == ProjectMemberRole.Editor)
        {
            return true;
        }

        if (!userRoles.Contains(ApplicationRole.User))
        {
            return false;
        }

        if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
        {
            return true;
        }

        return false;
    }

    private static bool CanEditProjectMembers(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, bool isRevision, ProjectMemberRole? projectMemberRole)
    {
        if (isRevision)
        {
            return false;
        }

        if (userRoles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (projectMemberRole == ProjectMemberRole.Editor)
        {
            return true;
        }

        if (!userRoles.Contains(ApplicationRole.User))
        {
            return false;
        }

        if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
        {
            return true;
        }

        return false;
    }
}
