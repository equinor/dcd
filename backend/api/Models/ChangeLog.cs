namespace api.Models;

public class ChangeLog
{
    public int Id { get; set; }
    public required string EntityName { get; set; }
    public string? PropertyName { get; set; }
    public required Guid EntityId { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public required string? Username { get; set; }
    public required DateTime TimestampUtc { get; set; }
    public required string EntityState { get; set; }
    public string? Payload { get; set; }
}
