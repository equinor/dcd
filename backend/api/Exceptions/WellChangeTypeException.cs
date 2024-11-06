namespace api.Exceptions;

public class WellChangeTypeException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
