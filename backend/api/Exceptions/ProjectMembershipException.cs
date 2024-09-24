namespace api.Exceptions;

public class ProjectMembershipException : Exception
{
    public Guid EntityId { get; }

    public ProjectMembershipException(string message, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
    }
}
