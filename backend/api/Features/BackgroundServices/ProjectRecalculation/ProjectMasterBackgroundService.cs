using api.Features.BackgroundServices.ProjectRecalculation.Services;

using static api.Features.BackgroundServices.DisableConcurrentJobExecution.DisableConcurrentJobExecutionService;

namespace api.Features.BackgroundServices.ProjectRecalculation;

public class ProjectRecalculationBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ProjectRecalculationBackgroundService> logger)
    : BackgroundService
{
    private readonly TimeSpan _executionFrequency = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_executionFrequency, stoppingToken);

            if (!IsJobRunnerInstance)
            {
                continue;
            }

            await RunProjectRecalculation();
        }
    }

    private async Task RunProjectRecalculation()
    {
        using var scope = scopeFactory.CreateScope();
        var recalculateProjectService = scope.ServiceProvider.GetRequiredService<RecalculateProjectService>();

        try
        {
            await recalculateProjectService.RecalculateProjects();
        }
        catch (Exception e)
        {
            logger.LogCritical("Project recalculation failed: {}", e);
        }
    }
}
