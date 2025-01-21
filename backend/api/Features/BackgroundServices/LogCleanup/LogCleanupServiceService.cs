using api.Context;

using Microsoft.EntityFrameworkCore;

using static api.Features.BackgroundServices.DisableConcurrentJobExecution.DisableConcurrentJobExecutionService;

namespace api.Features.BackgroundServices.LogCleanup;

public class JobCleanupService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly TimeSpan _executionFrequency = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_executionFrequency, stoppingToken);

            if (!IsJobRunnerInstance)
            {
                continue;
            }

            await DeleteOutdatedLogs();
        }
    }

    private async Task DeleteOutdatedLogs()
    {
        using var scope = serviceScopeFactory.CreateScope();

        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.ChangeTracker.LazyLoadingEnabled = false;

        try
        {
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

            await dbContext.FrontendExceptions
                .Where(x => x.CreatedUtc < oneHundredDaysAgo)
                .OrderBy(x => x.Id)
                .Take(500)
                .ExecuteDeleteAsync();
        }
        catch
        {
            // Ignored
        }
    }
}
