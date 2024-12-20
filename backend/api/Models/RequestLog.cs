namespace api.Models;

public class RequestLog
{
    public int Id { get; set; }
    public required string UrlPattern { get; set; }
    public required string Url { get; set; }
    public required string Verb { get; set; }
    public required long RequestLengthInMilliseconds { get; set; }
    public required DateTime RequestTimestampUtc { get; set; }
    public string? Username { get; set; }
}
