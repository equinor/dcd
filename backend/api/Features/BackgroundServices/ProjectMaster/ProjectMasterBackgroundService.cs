using api.AppInfrastructure;
using api.Features.BackgroundServices.ProjectMaster.Services;

namespace api.Features.BackgroundServices.ProjectMaster;

public class ProjectMasterBackgroundService(IServiceScopeFactory serviceScopeFactory)
    : DcdBackgroundService(serviceScopeFactory, executionFrequency: TimeSpan.FromHours(1))
{
    protected override async Task ExecuteJob()
    {
        if (!ShouldRunAtThisTimeOfDay())
        {
            return;
        }

        using var scope = ServiceScopeFactory.CreateScope();

        await scope.ServiceProvider.GetRequiredService<UpdateProjectFromProjectMasterService>().UpdateProjectFromProjectMaster();
    }

    private static bool ShouldRunAtThisTimeOfDay()
    {
        var (timeOfDayStart, timeOfDayEnd) = GetWindowWhenJobShouldRun();

        var utcNow = DateTime.UtcNow.TimeOfDay;

        return utcNow > timeOfDayStart && utcNow < timeOfDayEnd;
    }

    private static (TimeSpan timeOfDayUtcStart, TimeSpan timeOfDayUtcEnd) GetWindowWhenJobShouldRun()
    {
        return DcdEnvironments.RunProjectMasterBackgroundServiceHourly
            ? (TimeSpan.FromHours(0), TimeSpan.FromHours(23))
            : (TimeSpan.FromHours(13), TimeSpan.FromHours(14));
    }
}
