namespace api.Controllers;

public enum ActionType
{
    Read,
    Edit,
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class ActionTypeAttribute : Attribute
{
    public ActionType ActionType { get; }

    public ActionTypeAttribute(ActionType actionType)
    {
        ActionType = actionType;
    }
}
