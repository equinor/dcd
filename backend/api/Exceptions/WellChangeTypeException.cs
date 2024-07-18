namespace api.Exceptions;

public class WellChangeTypeException : Exception
{
    public Guid EntityId { get; }

    public WellChangeTypeException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
