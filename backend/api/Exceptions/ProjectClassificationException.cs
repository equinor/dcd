namespace api.Exceptions;

public class ProjectClassificationException : Exception
{
    public Guid EntityId { get; }

    public ProjectClassificationException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
