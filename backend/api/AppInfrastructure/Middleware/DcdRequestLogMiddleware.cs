using api.Context;
using api.Models.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure.Middleware;

public class DcdRequestLogMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context, IServiceScopeFactory serviceScopeFactory)
    {
        var requestStartUtc = DateTime.UtcNow;

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

            var requestEndUtc = DateTime.UtcNow;

            dbContext.RequestLogs.Add(new RequestLog
            {
                UrlPattern = urlPattern,
                Url = context.Request.Path,
                Verb = context.Request.Method,
                RequestLengthInMilliseconds = (int)requestEndUtc.Subtract(requestStartUtc).TotalMilliseconds,
                RequestStartUtc = requestStartUtc,
                RequestEndUtc = requestEndUtc
            });

            await dbContext.SaveChangesAsync();
        });

        return next(context);
    }
}
