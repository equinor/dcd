using api.AppInfrastructure.Authorization;
using api.Features.ProjectAccess.V2;
using api.Models;

using Xunit;

namespace tests.Features.ProjectAccess;

public class AccessCalculatorTests
{
    [Fact]
    public void User_should_have_view_access_to_project__when_project_classification_is_open()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_internal()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_restricted()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_confidential()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.View, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_be_able_to_create_revision__when_role_is_user_and_has_view_access_to_project_and_not_on_revision()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_be_able_to_create_revision__when_role_is_admin_and_has_view_access_to_project_and_not_on_revision()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.Admin]
        };

        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_not_be_able_to_create_revision__when_role_is_readonly()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.ReadOnly]
        };

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.CreateRevision, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_not_be_able_to_edit_project_data__when_role_is_readonly()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.ReadOnly]
        };

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
    }

    [Fact]
    public void User_should_be_able_to_edit_project_data__when_role_is_user()
    {
        var currentUser = new CurrentUser
        {
            Username = "test",
            UserId = Guid.Empty,
            Roles = [ApplicationRole.User]
        };

        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: true));
        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: true));

        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: false, userIsConnectedToProject: false));
        Assert.Contains(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: false, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: false, userIsConnectedToProject: false));

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: true));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: true));

        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Open, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Internal, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Restricted, isRevision: true, userIsConnectedToProject: false));
        Assert.DoesNotContain(AccessActions.EditProjectData, AccessCalculator.CalculateAccess(currentUser, ProjectClassification.Confidential, isRevision: true, userIsConnectedToProject: false));
    }
}
