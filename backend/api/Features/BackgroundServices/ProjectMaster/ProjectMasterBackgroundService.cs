using api.AppInfrastructure;
using api.Features.BackgroundServices.ProjectMaster.Services;

namespace api.Features.BackgroundServices.ProjectMaster;

public class ProjectMasterBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ProjectMasterBackgroundService> logger)
    : BackgroundService
{
    private readonly int _generalDelay = (int)TimeSpan.FromHours(1).TotalMilliseconds;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_generalDelay, stoppingToken);
            await UpdateProjects();
        }
    }

    private async Task UpdateProjects()
    {
        logger.LogInformation("ProjectMasterBackgroundService: Running");

        if (!ShouldRunAtThisTimeOfDay())
        {
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var updateService = scope.ServiceProvider.GetRequiredService<UpdateProjectFromProjectMasterService>();

        try
        {
            await updateService.UpdateProjectFromProjectMaster();

            logger.LogInformation($"Updated all projects from project master.");
        }
        catch (Exception e)
        {
            logger.LogCritical("Update from Project Master failed: {}", e);
        }
    }

    private static bool ShouldRunAtThisTimeOfDay()
    {
        var (timeOfDayUtcStart, timeOfDayUtcEnd) = GetWindowWhenJobShouldRun();

        var utcNow = DateTime.UtcNow.TimeOfDay;

        return utcNow > timeOfDayUtcStart && utcNow < timeOfDayUtcEnd;
    }

    private static (TimeSpan timeOfDayUtcStart, TimeSpan timeOfDayUtcEnd) GetWindowWhenJobShouldRun()
    {
        return DcdEnvironments.IsProd()
            ? (TimeSpan.FromHours(13), TimeSpan.FromHours(14))
            : (TimeSpan.FromHours(0), TimeSpan.FromHours(23));
    }
}
