namespace api.Exceptions;

public class ModifyRevisionException : Exception
{
    public Guid EntityId { get; }

    public ModifyRevisionException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
