namespace api.Exceptions;

public class MappingException : Exception
{
    public Guid EntityId { get; }

    public MappingException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
