using System.Net;
using System.Text.Json;

using api.Exceptions;

namespace api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate requestDelegate,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly ILogger _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
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
            case KeyNotFoundException:
            case NotFoundInDBException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;
            case WellChangeTypeException:
            case InvalidInputException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ProjectAccessMismatchException:
            case ProjectClassificationException:
            case ProjectMembershipException:
            case ModifyRevisionException:
                statusCode = HttpStatusCode.Forbidden;
                message = exception.Message;
                break;
            case ProjectAlreadyExistsException:
            case ResourceAlreadyExistsException:
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
