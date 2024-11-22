using System.Net;
using System.Text.Json;

using api.Exceptions;

namespace api.AppInfrastructure.Middleware;

public class DcdExceptionHandlingMiddleware(
    RequestDelegate requestDelegate,
    ILogger<DcdExceptionHandlingMiddleware> logger)
{
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
            logger.LogInformation(
                "Request {trace} {user} {method} {url} => {statusCode}",
                context.TraceIdentifier,
                context.User.Identity?.Name,
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode);
        }
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        logger.LogError(exception.ToString());

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case FusionOrgNotFoundException:
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
            case InputValidationException:
                statusCode = HttpStatusCode.UnprocessableContent;
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
