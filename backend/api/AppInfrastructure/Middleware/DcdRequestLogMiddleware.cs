using System.Diagnostics;

using api.AppInfrastructure.Authorization;
using api.Context;
using api.Models;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Middleware;

public class DcdRequestLogMiddleware(RequestDelegate next, CurrentUser currentUser)
{
    public Task Invoke(HttpContext context, IServiceScopeFactory serviceScopeFactory)
    {
        var stopwatch = Stopwatch.StartNew();

        var urlPattern = (context.Features.Get<IEndpointFeature>()?.Endpoint as RouteEndpoint)?.RoutePattern.RawText;

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
                RequestTimestampUtc = DateTime.UtcNow,
                Username = currentUser.Username
            });

            await dbContext.SaveChangesAsync();
        });

        return next(context);
    }
}
