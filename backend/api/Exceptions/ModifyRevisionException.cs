namespace api.Exceptions;

public class ModifyRevisionException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
