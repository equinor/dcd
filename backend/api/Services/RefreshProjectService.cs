namespace api.Services;

public class RefreshProjectService : BackgroundService
{
    private const int generalDelay = 1 * 1000 * 3600; // Each hour
    private readonly ILogger<RefreshProjectService> _logger;
    private readonly IConfiguration _configuration;

    private readonly IServiceScopeFactory _scopeFactory;
    public RefreshProjectService(IServiceScopeFactory scopeFactory, ILogger<RefreshProjectService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

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

        _logger.LogInformation("HostingService: Running");
        if (Showtime())
        {
            using var scope = _scopeFactory.CreateScope();
            var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();
            try
            {
                projectService.UpdateProjectFromProjectMaster();
            }
            catch (Exception e)
            {
                _logger.LogCritical("Update from Project Master failed: {}", e);
            }
        }
        return Task.FromResult("Done");
    }

    private bool Showtime()
    {
        var runtime = _configuration.GetSection("HostedService").GetValue<string>("RunTime");
        var hour = Int32.Parse(runtime.Split(':')[0]);
        var minute = Int32.Parse(runtime.Split(':')[1]);
        var second = Int32.Parse(runtime.Split(':')[2]);
        TimeSpan start = new TimeSpan(hour, minute, second);
        TimeSpan end = new TimeSpan(hour + 1, minute, second);
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now > start) && (now < end))
        {
            _logger.LogInformation("HostingService: Running Update Project from Project Master");
            return true;
        }

        return false;
    }
}
