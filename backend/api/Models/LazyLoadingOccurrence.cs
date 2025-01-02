namespace api.Models;

public class LazyLoadingOccurrence
{
    public int Id { get; set; }
    public required string Message { get; set; }
    public required DateTime TimestampUtc { get; set; }
}
