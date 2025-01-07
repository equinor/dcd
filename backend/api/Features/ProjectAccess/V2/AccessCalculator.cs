using api.AppInfrastructure.Authorization;
using api.Models;

namespace api.Features.ProjectAccess.V2;

public static class AccessCalculator
{
    private static List<string> AllRevisionAccesses => [AccessActions.View];
    private static List<string> AllProjectAccesses => [AccessActions.View, AccessActions.CreateRevision];

    private static List<string> AllReadRevisionAccesses => [AccessActions.View];
    private static List<string> AllReadProjectAccesses => [AccessActions.View];

    public static List<string> CalculateAccess(CurrentUser currentUser, ProjectClassification projectClassification, bool isRevision, bool userIsConnectedToProject)
    {
        if (currentUser.Roles.Contains(ApplicationRole.Admin))
        {
            return isRevision
                ? AllRevisionAccesses
                : AllProjectAccesses;
        }

        if (currentUser.Roles.Count == 1 && currentUser.Roles.Contains(ApplicationRole.ReadOnly))
        {
            return isRevision
                ? AllReadRevisionAccesses
                : AllReadProjectAccesses;
        }

        if (projectClassification == ProjectClassification.Open)
        {
            return isRevision
                ? AllRevisionAccesses
                : AllProjectAccesses;
        }

        if (userIsConnectedToProject)
        {
            return isRevision
                ? AllRevisionAccesses
                : AllProjectAccesses;
        }

        return [];
    }
}
