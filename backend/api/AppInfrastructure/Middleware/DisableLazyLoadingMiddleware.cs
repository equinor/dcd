using api.AppInfrastructure.ControllerAttributes;
using api.Context;

namespace api.AppInfrastructure.Middleware;

public class DisableLazyLoadingMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        var executingEndpoint = context.GetEndpoint();

        var attributes = executingEndpoint?.Metadata.OfType<DisableLazyLoadingAttribute>().ToList();

        if (attributes != null && attributes.Any())
        {
            var dbContext = serviceProvider.GetRequiredService<DcdDbContext>();

            dbContext.ChangeTracker.LazyLoadingEnabled = false;
        }

        return next(context);
    }
}
