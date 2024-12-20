using System.Diagnostics;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Middleware;

public class DcdRequestLogMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context, IServiceScopeFactory serviceScopeFactory)
    {
        var stopwatch = Stopwatch.StartNew();

        var endpoint = context.GetEndpoint();
        var routeEndpoint = endpoint as RouteEndpoint;
        var routePattern = routeEndpoint?.RoutePattern;
        var urlPattern = routePattern?.RawText;

        if (urlPattern == null)
        {
            return next(context);
        }

        context.Response.OnCompleted(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            dbContext.RequestLogs.Add(new RequestLog
            {
                UrlPattern = urlPattern,
                Url = context.Request.Path,
                Verb = context.Request.Method,
                RequestLengthInMilliseconds = stopwatch.ElapsedMilliseconds,
                RequestTimestampUtc = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
        });

        return next(context);
    }
}
