namespace api.Controllers;

public enum ActionType
{
    Read,
    Edit,
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class ActionTypeAttribute(ActionType actionType) : Attribute
{
    public ActionType ActionType { get; } = actionType;
}
