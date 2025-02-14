using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.ControllerAttributes;
using api.Features.ProjectAccess;
using api.Models.Enums;

using Xunit;

namespace tests.Features.ProjectAccess;

public class UserActionCalculatorTests
{
    [Fact]
    public void User_should_have_view_access_to_project__when_project_classification_is_open()
    {
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, null));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, null));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_internal()
    {
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, null));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, null));
    }

    [Fact]
    public void Admin_should_have_view_access_to_project__when_project_classification_is_confidential()
    {
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: true, null));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: false, null));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_restricted()
    {
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, null));
    }

    [Fact]
    public void User_should_have_view_access_to_project__only_when_user_is_connected_to_project__when_project_classification_is_confidential()
    {
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Read, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, null));
    }

    [Fact]
    public void User_should_be_able_to_create_revision__when_role_is_user_and_has_view_access_to_project_and_not_on_revision()
    {
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, null));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, null));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, null));
    }

    [Fact]
    public void User_should_be_able_to_create_revision__when_role_is_admin_and_has_view_access_to_project_and_not_on_revision()
    {
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Open, isRevision: false, null));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Internal, isRevision: false, null));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Restricted, isRevision: false, null));
        Assert.Contains(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: false, null));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Open, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Internal, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.Admin], ProjectClassification.Confidential, isRevision: true, null));
    }

    [Fact]
    public void User_should_not_be_able_to_create_revision__when_role_is_readonly()
    {
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: false, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: false, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: false, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: false, null));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.CreateRevision, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: true, null));
    }

    [Fact]
    public void User_should_not_be_able_to_edit_project_data__when_role_is_readonly()
    {
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: false, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: false, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: false, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: false, null));

        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Open, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Internal, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.ReadOnly], ProjectClassification.Confidential, isRevision: true, null));
    }

    [Fact]
    public void User_should_be_able_to_edit_project_data__when_role_is_user()
    {
        Assert.Contains(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, ProjectMemberRole.Observer));
        Assert.Contains(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, ProjectMemberRole.Observer));

        Assert.Contains(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: false, null));
        Assert.Contains(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: false, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: false, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: false, null));

        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, ProjectMemberRole.Observer));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, ProjectMemberRole.Observer));

        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Open, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Internal, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Restricted, isRevision: true, null));
        Assert.DoesNotContain(ActionType.Edit, AccessCalculator.CalculateAccess([ApplicationRole.User], ProjectClassification.Confidential, isRevision: true, null));
    }
}
