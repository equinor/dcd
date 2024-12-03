namespace api.AppInfrastructure.ControllerAttributes;

public enum ActionType
{
    Read,
    Edit,
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ActionTypeAttribute(ActionType actionType) : Attribute
{
    public ActionType ActionType { get; } = actionType;
}
