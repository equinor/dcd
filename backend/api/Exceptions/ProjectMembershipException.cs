namespace api.Exceptions;

public class ProjectMembershipException(string message, Guid entityId) : Exception(message)
{
    public Guid EntityId { get; } = entityId;
}
