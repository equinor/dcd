using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.BackgroundServices.LogCleanup;

public class JobCleanupBackgroundService(IServiceScopeFactory serviceScopeFactory)
    : DcdBackgroundService(serviceScopeFactory, executionFrequency: TimeSpan.FromMinutes(5))
{
    protected override async Task ExecuteJob()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oneHundredDaysAgo = DateTime.UtcNow.Subtract(TimeSpan.FromDays(100));

        await dbContext.RequestLogs
            .Where(x => x.RequestStartUtc < oneHundredDaysAgo)
            .OrderBy(x => x.Id)
            .Take(500)
            .ExecuteDeleteAsync();

        await dbContext.ExceptionLogs
            .Where(x => x.UtcTimestamp < oneHundredDaysAgo)
            .OrderBy(x => x.Id)
            .Take(500)
            .ExecuteDeleteAsync();

        var sevenDaysAgo = DateTime.UtcNow.Subtract(TimeSpan.FromDays(7));

        await dbContext.BackgroundJobLogs
            .Where(x => x.TimestampUtc < sevenDaysAgo)
            .OrderBy(x => x.Id)
            .Take(500)
            .ExecuteDeleteAsync();
    }
}
