using System.Net;
using System.Text.Json;

using api.Exceptions;

namespace api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = requestDelegate;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
        finally
        {
            _logger.LogInformation(
            "Request {trace} {user} {method} {url} => {statusCode}",
            context.TraceIdentifier,
            context.User?.Identity?.Name,
            context.Request?.Method,
            context.Request?.Path.Value,
            context.Response?.StatusCode);
        }
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        _logger.LogError(exception.ToString());

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case NotFoundInDBException _:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case UnauthorizedAccessException _:
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;
            case InvalidInputException _:
            case WellChangeTypeException _:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ProjectAccessMismatchException _:
            case ProjectClassificationException _:
            case ProjectMembershipException _:
            case ModifyRevisionException _:
                statusCode = HttpStatusCode.Forbidden;
                message = exception.Message;
                break;
            case ResourceAlreadyExistsException _:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = message
        };

        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}
