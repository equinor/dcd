using System.Diagnostics;

namespace api.AppInfrastructure.Middleware;

public class DcdResponseTimerMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers["x-response-time-ms"] = stopwatch.ElapsedMilliseconds.ToString();

            return Task.CompletedTask;
        });

        return next(context);
    }
}
