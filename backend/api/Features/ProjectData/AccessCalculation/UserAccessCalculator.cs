using api.AppInfrastructure.Authorization;
using api.Models;

namespace api.Features.ProjectData.AccessCalculation;

public static class AccessCalculator
{
    public static List<string> CalculateAccess(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, bool isRevision, ProjectMemberRole? projectMemberRole)
    {
        var actions = new HashSet<string>();

        if (CanView(userRoles, projectClassification, projectMemberRole))
        {
            actions.Add(UserActions.View);
        }

        if (CanCreateRevision(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(UserActions.CreateRevision);
        }

        if (CanEditProjectData(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(UserActions.EditProjectData);
        }

        if (CanEditProjectMembers(userRoles, projectClassification, isRevision, projectMemberRole))
        {
            actions.Add(UserActions.EditProjectMembers);
        }

        return actions.ToList();
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

        if (userRoles.Contains(ApplicationRole.User))
        {
            if (projectMemberRole != null)
            {
                return true;
            }

            if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
            {
                return true;
            }
        }

        return false;
    }

    private static bool CanView(HashSet<ApplicationRole> userRoles, ProjectClassification projectClassification, ProjectMemberRole? projectMemberRole)
    {
        if (userRoles.Count == 0)
        {
            return false;
        }

        return projectClassification is ProjectClassification.Open or ProjectClassification.Internal || projectMemberRole != null;
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

        if (userRoles.Contains(ApplicationRole.User))
        {
            if (projectMemberRole != null)
            {
                return true;
            }

            if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
            {
                return true;
            }
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

        if (userRoles.Contains(ApplicationRole.User))
        {
            if (projectMemberRole != null)
            {
                return true;
            }

            if (projectClassification is ProjectClassification.Open or ProjectClassification.Internal)
            {
                return true;
            }
        }

        return false;
    }
}
