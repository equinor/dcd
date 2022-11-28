namespace api.Services;

public class RefreshProjectService : BackgroundService
{
    private const int generalDelay = 1 * 60 * 1000; // 10 seconds
    private readonly ILogger<RefreshProjectService> _logger;
    private readonly IConfiguration _configuration;
    public RefreshProjectService(ILogger<RefreshProjectService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
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
        if (Showtime())
        {
            // here i can write logic for taking backup at midnight
            Console.WriteLine("Executing background task");
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
        TimeSpan end = new TimeSpan(hour, minute + 1, second);
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now > start) && (now < end))
        {
            _logger.LogInformation("HostingService: Showtime");
            return true;
        }

        return false;
    }
}
