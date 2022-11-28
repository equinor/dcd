namespace api.Services;

public class RefreshProjectService : BackgroundService
{
    private const int generalDelay = 1 * 10 * 1000; // 10 seconds

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(generalDelay, stoppingToken);
            await UpdateProjects();
        }
    }

    private static Task UpdateProjects()
    {
        // here i can write logic for taking backup at midnight
        Console.WriteLine("Executing background task");

        return Task.FromResult("Done");
    }
}
