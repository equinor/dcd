namespace api.Models.Infrastructure;

public class ExceptionLog
{
    public int Id { get; set; }
    public required DateTime UtcTimestamp { get; set; }
    public required int HttpStatusCode { get; set; }
    public required string Method { get; set; }
    public required string RequestBody { get; set; }
    public required string? RequestUrl { get; set; }
    public required string DisplayUrl { get; set; }
    public required string Environment { get; set; }
    public required string? StackTrace { get; set; }
    public required string ExceptionMessage { get; set; }
    public required string? InnerExceptionStackTrace { get; set; }
    public required string? InnerExceptionMessage { get; set; }
}
