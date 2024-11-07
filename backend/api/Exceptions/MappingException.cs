namespace api.Exceptions;

public class MappingException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
