using System.Diagnostics;

using api.Features.BackgroundServices.ProjectMaster.Services;

namespace api.Features.BackgroundServices.ProjectMaster;

public class ProjectMasterBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<ProjectMasterBackgroundService> logger,
    IConfiguration configuration)
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
        logger.LogInformation("HostingService: Running");

        if (Showtime())
        {
            using var scope = scopeFactory.CreateScope();
            var updateService = scope.ServiceProvider.GetRequiredService<UpdateProjectFromProjectMasterService>();

            try
            {
                var stopwatch = Stopwatch.StartNew();

                await updateService.UpdateProjectFromProjectMaster();

                logger.LogInformation($"Updated all projects from project master in {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception e)
            {
                logger.LogCritical("Update from Project Master failed: {}", e);
            }
        }
    }

    private bool Showtime()
    {
        var runtime = configuration.GetSection("HostedService").GetValue<string>("RunTime");

        if (string.IsNullOrEmpty(runtime))
        {
            logger.LogInformation("HostingService: No runtime specified");
            return false;
        }

        var hour = int.Parse(runtime.Split(':')[0]);
        var minute = int.Parse(runtime.Split(':')[1]);
        var second = int.Parse(runtime.Split(':')[2]);
        var start = new TimeSpan(hour, minute, second);
        var end = new TimeSpan(hour + 1, minute, second);
        var now = DateTime.Now.TimeOfDay;

        if (now > start && now < end)
        {
            logger.LogInformation("HostingService: Running Update Project from Project Master");
            return true;
        }

        return false;
    }
}
