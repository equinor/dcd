namespace api.Models;

public class ChangeLog
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = null!;
    public string? PropertyName { get; set; }
    public Guid EntityId { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Username { get; set; }
    public DateTime TimestampUtc { get; set; }
    public required string EntityState { get; set; }
    public string? Payload { get; set; }
}
