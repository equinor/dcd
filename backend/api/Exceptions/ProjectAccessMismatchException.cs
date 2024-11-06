namespace api.Exceptions;

public class ProjectAccessMismatchException : Exception
{
    public Guid EntityId { get; }
    public Guid UrlProjectId { get; }

    public ProjectAccessMismatchException(string message) : base(message)
    {
    }

    public ProjectAccessMismatchException(string message, Guid urlProjectId, Guid entityId)
        : base(message)
    {
        EntityId = entityId;
        UrlProjectId = urlProjectId;
    }

    public ProjectAccessMismatchException(string message, Guid urlProjectId)
    : base(message)
    {
        UrlProjectId = urlProjectId;
    }
}
