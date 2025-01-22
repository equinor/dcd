using api.AppInfrastructure;
using api.Context;
using api.Models.Infrastructure.BackgroundJobs;

using Microsoft.EntityFrameworkCore;

namespace api.Features.BackgroundServices.DisableConcurrentJobExecution;

public class DisableConcurrentJobExecutionService(IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    public static bool IsJobRunnerInstance { get; private set; }
    private static readonly TimeSpan Frequency = TimeSpan.FromSeconds(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Frequency, stoppingToken);
            
            try
            {
                await DeleteDeadInstances();
                await UpdateLastSeenOnCurrentInstance(Environment.MachineName);

                IsJobRunnerInstance = await CheckIfJobRunnerInstance(Environment.MachineName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private async Task DeleteDeadInstances()
    {
        using var scope = scopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<DcdDbContext>();

        var threshold = DateTime.UtcNow.Subtract(Frequency * 3);

        var allInstances = await context.BackgroundJobMachineInstanceLogs
                                        .Where(x => x.IsJobRunner || x.LastSeenUtc < threshold)
                                        .ToListAsync();

        if (allInstances.Count(x => x.IsJobRunner) > 1)
        {
            context.BackgroundJobMachineInstanceLogs.RemoveRange(allInstances);
            await context.SaveChangesAsync();

            return;
        }

        foreach (var deadInstance in allInstances.Where(x => x.LastSeenUtc < threshold))
        {
            context.BackgroundJobMachineInstanceLogs.Remove(deadInstance);
        }

        await context.SaveChangesAsync();
    }

    private async Task UpdateLastSeenOnCurrentInstance(string machineName)
    {
        using var scope = scopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<DcdDbContext>();

        var instance = await context.BackgroundJobMachineInstanceLogs.SingleOrDefaultAsync(x => x.MachineName == machineName);

        if (instance != null)
        {
            instance.LastSeenUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return;
        }

        context.BackgroundJobMachineInstanceLogs.Add(new BackgroundJobMachineInstanceLog
        {
            MachineName = machineName,
            IsJobRunner = false,
            LastSeenUtc = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    private async Task<bool> CheckIfJobRunnerInstance(string machineName)
    {
        if (DcdEnvironments.RunBackgroundJobsOnLocalMachine)
        {
            return true;
        }

        using var scope = scopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<DcdDbContext>();

        var instance = await context.BackgroundJobMachineInstanceLogs.FirstOrDefaultAsync(x => x.IsJobRunner);

        if (instance != null)
        {
            return instance.MachineName == machineName;
        }

        var self = await context.BackgroundJobMachineInstanceLogs.SingleAsync(x => x.MachineName == machineName);
        self.IsJobRunner = true;

        await context.SaveChangesAsync();

        return true;
    }
}
