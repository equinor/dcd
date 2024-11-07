namespace api.Services;

public class RefreshProjectService(
    IServiceScopeFactory scopeFactory,
    ILogger<RefreshProjectService> logger,
    IConfiguration configuration)
    : BackgroundService
{
    private const int generalDelay = 1 * 1000 * 3600; // Each hour

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(generalDelay, stoppingToken);
            await UpdateProjects();
        }
    }

    private Task UpdateProjects()
    {
        logger.LogInformation("HostingService: Running");
        if (Showtime())
        {
            using var scope = scopeFactory.CreateScope();
            var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
            try
            {
                projectService.UpdateProjectFromProjectMaster();
            }
            catch (Exception e)
            {
                logger.LogCritical("Update from Project Master failed: {}", e);
            }
        }

        return Task.FromResult("Done");
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

        if ((now > start) && (now < end))
        {
            logger.LogInformation("HostingService: Running Update Project from Project Master");
            return true;
        }

        return false;
    }
}
