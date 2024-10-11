namespace api.Exceptions;

public class ProjectAlreadyExistsException : Exception
{
    public ProjectAlreadyExistsException(string message) : base(message)
    {
    }
}
