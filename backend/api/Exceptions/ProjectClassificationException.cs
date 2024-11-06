namespace api.Exceptions;

public class ProjectClassificationException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
