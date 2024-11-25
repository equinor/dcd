using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using api.Exceptions;

using Microsoft.AspNetCore.Http.Extensions;

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

    private async Task HandleException(HttpContext context, Exception exception)
    {
        logger.LogError(exception.ToString());

        HttpStatusCode statusCode;
        var errorInformation = new Dictionary<string, string>();

        switch (exception)
        {
            case FusionOrgNotFoundException:
            case KeyNotFoundException:
            case NotFoundInDBException:
                statusCode = HttpStatusCode.NotFound;
                errorInformation.Add("message", exception.Message);
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                errorInformation.Add("message", exception.Message);
                break;
            case WellChangeTypeException:
            case InvalidInputException:
                statusCode = HttpStatusCode.BadRequest;
                errorInformation.Add("message", exception.Message);
                break;
            case InputValidationException:
                statusCode = HttpStatusCode.UnprocessableContent;
                errorInformation.Add("message", exception.Message);
                break;
            case ProjectAccessMismatchException:
            case ProjectClassificationException:
            case ProjectMembershipException:
            case ModifyRevisionException:
                statusCode = HttpStatusCode.Forbidden;
                errorInformation.Add("message", exception.Message);
                break;
            case ProjectAlreadyExistsException:
            case ResourceAlreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                errorInformation.Add("message", exception.Message);
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                errorInformation.Add("message", "An unexpected error occurred");
                break;
        }

        if (DcdEnvironments.ReturnExceptionDetails)
        {
            AppendDebugInfo(errorInformation,
                exception,
                context.Request.GetDisplayUrl(),
                context.Request.Method,
                await GetBody(context.Request.Body),
                context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(errorInformation);

        await context.Response.WriteAsync(jsonResponse);
    }

    private static void AppendDebugInfo(Dictionary<string, string> errorInformation, Exception exception, string? url, string? method, string? body, string? user)
    {
        errorInformation.Add("Environment", DcdEnvironments.CurrentEnvironment);

        if (exception.StackTrace != null)
        {
            errorInformation.Add("stacktrace", exception.StackTrace);
        }

        errorInformation.Add("ExceptionMessage", exception.Message);

        if (exception.InnerException != null)
        {
            errorInformation.Add("InnerExceptionMessage", exception.InnerException.Message);
            
            if (exception.InnerException.StackTrace != null)
            {
                errorInformation.Add("InnerExceptionStacktrace", exception.InnerException.StackTrace);
            }
        }

        if (url != null)
        {
            errorInformation.Add("Url", url);
        }

        if (method != null)
        {
            errorInformation.Add("Method", method);
        }

        if (!string.IsNullOrEmpty(body))
        {
            errorInformation.Add("Body", body);
        }

        if (user != null)
        {
            errorInformation.Add("User", user);
        }
    }

    private static async Task<string> GetBody(Stream requestStream)
    {
        requestStream.Position = 0;
        using var reader = new StreamReader(requestStream, Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        return body;
    }
}
