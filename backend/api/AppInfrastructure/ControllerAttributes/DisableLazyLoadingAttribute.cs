namespace api.AppInfrastructure.ControllerAttributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DisableLazyLoadingAttribute : Attribute;
