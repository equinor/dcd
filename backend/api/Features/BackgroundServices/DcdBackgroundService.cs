using System.Diagnostics;

using api.Context;
using api.Models.Infrastructure.BackgroundJobs;

using Microsoft.EntityFrameworkCore;

using static api.Features.BackgroundServices.DisableConcurrentJobExecution.DisableConcurrentJobExecutionService;

namespace api.Features.BackgroundServices;

public abstract class DcdBackgroundService(IServiceScopeFactory serviceScopeFactory, TimeSpan executionFrequency) : BackgroundService
{
    protected abstract Task ExecuteJob();

    protected IServiceScopeFactory ServiceScopeFactory => serviceScopeFactory;
    private TimeSpan ExecutionFrequency => executionFrequency;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(ExecutionFrequency, stoppingToken);

            if (!IsJobRunnerInstance)
            {
                continue;
            }

            var stopwatch = Stopwatch.StartNew();
            string? exceptionMessage = null;

            try
            {
                await ExecuteJob();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            try
            {
                await LogJobRun(stopwatch.ElapsedMilliseconds, exceptionMessage);
            }
            catch
            {
                // Ignored
            }
        }
    }

    private async Task LogJobRun(long elapsedMilliseconds, string? exceptionMessage)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DcdDbContext>>();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        dbContext.BackgroundJobLogs.Add(new BackgroundJobLog
        {
            JobType = GetType().Name,
            MachineName = Environment.MachineName,
            Success = exceptionMessage == null,
            ExceptionMessage = exceptionMessage,
            ExecutionDurationInMilliseconds = elapsedMilliseconds,
            TimestampUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }
}
