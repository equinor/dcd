namespace api.Exceptions;

public class InvalidInputException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
