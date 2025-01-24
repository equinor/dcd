using Microsoft.AspNetCore.Authorization;

namespace api.AppInfrastructure.ControllerAttributes;

public enum ActionType
{
    Read,
    Edit,
    CreateRevision,
    EditProjectMembers
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeActionTypeAttribute(ActionType actionType) : AuthorizeAttribute
{
    public ActionType ActionType { get; } = actionType;
}
