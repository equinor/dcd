namespace api.Exceptions;

public class InvalidInputException : Exception
{
    public Guid EntityId { get; }

    public InvalidInputException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
