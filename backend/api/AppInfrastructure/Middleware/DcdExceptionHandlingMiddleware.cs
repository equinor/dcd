using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using api.Context;
using api.Exceptions;
using api.Models.Infrastructure;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Middleware;

public class DcdExceptionHandlingMiddleware(
    RequestDelegate requestDelegate,
    ILogger<DcdExceptionHandlingMiddleware> logger,
    IServiceScopeFactory serviceScopeFactory)
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

        var body = await GetBody(context.Request.Body);

        await SaveExceptionToDatabase(exception, httpStatusCode, context, body);

        if (DcdEnvironments.ReturnExceptionDetails)
        {
            AddIfNotNull(errorInformation, "Environment", DcdEnvironments.CurrentEnvironment);
            AddIfNotNull(errorInformation, "ExceptionStackTrace", exception.StackTrace);
            AddIfNotNull(errorInformation, "ExceptionMessage", exception.Message);
            AddIfNotNull(errorInformation, "InnerExceptionStackTrace", exception.InnerException?.StackTrace);
            AddIfNotNull(errorInformation, "InnerExceptionMessage", exception.InnerException?.Message);
            AddIfNotNull(errorInformation, "Url", context.Request.GetDisplayUrl());
            AddIfNotNull(errorInformation, "Method", context.Request.Method);
            AddIfNotNull(errorInformation, "Body", body);
            AddIfNotNull(errorInformation, "User", context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)httpStatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorInformation));
    }

    private static void AddIfNotNull(Dictionary<string, string> errorInformation, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            errorInformation.Add(key, value);
        }
    }

    private async Task SaveExceptionToDatabase(Exception exception, HttpStatusCode httpStatusCode, HttpContext httpContext, string body)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        dbContext.ExceptionLogs.Add(new ExceptionLog
        {
            Environment = DcdEnvironments.CurrentEnvironment,
            UtcTimestamp = DateTime.UtcNow,
            HttpStatusCode = (int)httpStatusCode,
            DisplayUrl = httpContext.Request.GetDisplayUrl(),
            RequestUrl = httpContext.Request.Path.Value,
            Method = httpContext.Request.Method,
            RequestBody = body,
            StackTrace = exception.StackTrace,
            ExceptionMessage = exception.Message,
            InnerExceptionStackTrace = exception.InnerException?.StackTrace,
            InnerExceptionMessage = exception.InnerException?.Message
        });

        await dbContext.SaveChangesAsync();
    }

    private static (HttpStatusCode httpStatusCode, string exceptionMessage) GetHttpStatusCodeAndExceptionMessage(Exception exception)
    {
        switch (exception)
        {
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
            case ModifyRevisionException:
                return (HttpStatusCode.Forbidden, exception.Message);

            case ProjectAlreadyExistsException:
            case ResourceAlreadyExistsException:
                return (HttpStatusCode.Conflict, exception.Message);

            default:
                return (HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task<string> GetBody(Stream requestStream)
    {
        requestStream.Position = 0;
        using var reader = new StreamReader(requestStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
