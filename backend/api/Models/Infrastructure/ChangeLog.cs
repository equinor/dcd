namespace api.Models.Infrastructure;

public class ChangeLog
{
    public int Id { get; set; }
    public required string EntityName { get; set; }
    public required string? PropertyName { get; set; }
    public required Guid EntityId { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
    public required string? Username { get; set; }
    public required DateTime TimestampUtc { get; set; }
    public required string EntityState { get; set; }
    public required string? Payload { get; set; }
}
