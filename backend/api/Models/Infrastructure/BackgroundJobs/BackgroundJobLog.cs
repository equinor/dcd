namespace api.Models.Infrastructure.BackgroundJobs;

public class BackgroundJobLog
{
    public int Id { get; set; }
    public required string JobType { get; set; }
    public required string MachineName { get; set; }
    public required bool Success { get; set; }
    public required string? ExceptionMessage { get; set; }
    public required long ExecutionDurationInMilliseconds { get; set; }
    public required DateTime TimestampUtc { get; set; }
}
