namespace api.Models.Infrastructure.BackgroundJobs;

public class BackgroundJobMachineInstanceLog
{
    public int Id { get; set; }
    public required string MachineName { get; set; }
    public required DateTime LastSeenUtc { get; set; }
    public required bool IsJobRunner { get; set; }
}
