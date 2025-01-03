using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Middleware;

public class DcdExceptionHandlingMiddleware(
    RequestDelegate requestDelegate,
    ILogger<DcdExceptionHandlingMiddleware> logger,
    IDbContextFactory<DcdDbContext> contextFactory)
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

        var (httpStatusCode, exceptionMessage) = GetHttpStatusCodeAndExceptionMessage(exception);

        var errorInformation = new Dictionary<string, string>
        {
            { "message", exceptionMessage }
        };

        await SaveExceptionToDatabase(exception, httpStatusCode, context);

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
        context.Response.StatusCode = (int)httpStatusCode;

        var jsonResponse = JsonSerializer.Serialize(errorInformation);

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task SaveExceptionToDatabase(Exception exception, HttpStatusCode httpStatusCode, HttpContext httpContext)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        dbContext.ExceptionLogs.Add(new ExceptionLog
        {
            Environment = DcdEnvironments.CurrentEnvironment,
            UtcTimestamp = DateTime.UtcNow,
            HttpStatusCode = (int)httpStatusCode,
            DisplayUrl = httpContext.Request.GetDisplayUrl(),
            RequestUrl = httpContext.Request.Path.Value,
            Method = httpContext.Request.Method,
            RequestBody = await GetBody(httpContext.Request.Body),
            StackTrace = exception.StackTrace,
            ExceptionMessage = exception.Message,
            InnerExceptionStackTrace = exception.InnerException?.StackTrace,
            InnerExceptionMessage = exception.InnerException?.Message
        });

        await dbContext.SaveChangesAsync();
    }

    private static (HttpStatusCode, string exceptionMessage) GetHttpStatusCodeAndExceptionMessage(Exception exception)
    {
        switch (exception)
        {
            case FusionOrgNotFoundException:
            case KeyNotFoundException:
            case NotFoundInDbException:
                return (HttpStatusCode.NotFound, exception.Message);

            case UnauthorizedAccessException:
                return (HttpStatusCode.Unauthorized, exception.Message);

            case WellChangeTypeException:
            case InvalidInputException:
            case InvalidProjectIdException:
                return (HttpStatusCode.BadRequest, exception.Message);

            case InputValidationException:
                return (HttpStatusCode.UnprocessableContent, exception.Message);

            case ProjectAccessMismatchException:
            case ProjectClassificationException:
            case ProjectMembershipException:
            case ModifyRevisionException:
                return (HttpStatusCode.Forbidden, exception.Message);

            case ProjectAlreadyExistsException:
            case ResourceAlreadyExistsException:
                return (HttpStatusCode.Conflict, exception.Message);

            default:
                return (HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
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
