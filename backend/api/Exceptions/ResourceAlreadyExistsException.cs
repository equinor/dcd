namespace api.Exceptions;


public class ResourceAlreadyExistsException : Exception
{
    public ResourceAlreadyExistsException()
    {
    }

    public ResourceAlreadyExistsException(string message)
        : base(message)
    {
    }

    public ResourceAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
