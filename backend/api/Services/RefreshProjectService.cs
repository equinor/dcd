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
            var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();
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
        if (string.IsNullOrEmpty(runtime))
        {
            _logger.LogInformation("HostingService: No runtime specified");
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
            _logger.LogInformation("HostingService: Running Update Project from Project Master");
            return true;
        }

        return false;
    }
}
