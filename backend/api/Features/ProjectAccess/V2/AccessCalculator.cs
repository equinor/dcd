using api.AppInfrastructure.Authorization;
using api.Models;

namespace api.Features.ProjectAccess.V2;

public static class AccessCalculator
{
    public static List<string> CalculateAccess(CurrentUser currentUser, ProjectClassification projectClassification, bool isRevision, bool userIsConnectedToProject)
    {
        var actions = new HashSet<string>();

        if (CanView(currentUser, projectClassification, userIsConnectedToProject))
        {
            actions.Add(AccessActions.View);
        }

        if (CanCreateRevision(currentUser, projectClassification, isRevision, userIsConnectedToProject))
        {
            actions.Add(AccessActions.CreateRevision);
        }

        if (CanEditProjectData(currentUser, projectClassification, isRevision, userIsConnectedToProject))
        {
            actions.Add(AccessActions.EditProjectData);
        }

        if (CanEditProjectMembers(currentUser, projectClassification, isRevision, userIsConnectedToProject))
        {
            actions.Add(AccessActions.EditProjectMembers);
        }

        return actions.ToList();
    }

    private static bool CanCreateRevision(CurrentUser currentUser, ProjectClassification projectClassification, bool isRevision, bool userIsConnectedToProject)
    {
        if (isRevision)
        {
            return false;
        }

        if (currentUser.Roles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (currentUser.Roles.Contains(ApplicationRole.User))
        {
            if (userIsConnectedToProject)
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

    private static bool CanView(CurrentUser currentUser, ProjectClassification projectClassification, bool userIsConnectedToProject)
    {
        if (currentUser.Roles.Count == 0)
        {
            return false;
        }

        return projectClassification is ProjectClassification.Open or ProjectClassification.Internal || userIsConnectedToProject;
    }

    private static bool CanEditProjectData(CurrentUser currentUser, ProjectClassification projectClassification, bool isRevision, bool userIsConnectedToProject)
    {
        if (isRevision)
        {
            return false;
        }

        if (currentUser.Roles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (currentUser.Roles.Contains(ApplicationRole.User))
        {
            if (userIsConnectedToProject)
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

    private static bool CanEditProjectMembers(CurrentUser currentUser, ProjectClassification projectClassification, bool isRevision, bool userIsConnectedToProject)
    {
        if (isRevision)
        {
            return false;
        }

        if (currentUser.Roles.Contains(ApplicationRole.Admin))
        {
            return true;
        }

        if (currentUser.Roles.Contains(ApplicationRole.User))
        {
            if (userIsConnectedToProject)
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
