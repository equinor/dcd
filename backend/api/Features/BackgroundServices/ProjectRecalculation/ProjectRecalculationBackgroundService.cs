using api.Features.BackgroundServices.ProjectRecalculation.Services;

namespace api.Features.BackgroundServices.ProjectRecalculation;

public class ProjectRecalculationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    : DcdBackgroundService(serviceScopeFactory, executionFrequency: TimeSpan.FromSeconds(5))
{
    protected override async Task ExecuteJob()
    {
        using var scope = ServiceScopeFactory.CreateScope();

        await scope.ServiceProvider.GetRequiredService<RecalculateProjectService>().RecalculateProjects();
    }
}
