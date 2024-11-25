namespace api.Features.Prosp.Exceptions;

public class AccessDeniedException(string message, Exception innerException) : Exception(message, innerException);
