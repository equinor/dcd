namespace api.Models.Infrastructure;

public class RequestLog
{
    public int Id { get; set; }
    public required string UrlPattern { get; set; }
    public required string Url { get; set; }
    public required string Verb { get; set; }
    public required long RequestLengthInMilliseconds { get; set; }
    public required DateTime RequestStartUtc { get; set; }
    public required DateTime RequestEndUtc { get; set; }
}
