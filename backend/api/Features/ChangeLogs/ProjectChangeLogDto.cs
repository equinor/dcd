using System.ComponentModel.DataAnnotations;

namespace api.Features.ChangeLogs;

public class ProjectChangeLogDto
{
    [Required] public required Guid EntityId { get; set; }
    [Required] public required string EntityName { get; set; }
    public required string? PropertyName { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
    public required string? Username { get; set; }
    [Required] public required DateTime TimestampUtc { get; set; }
    [Required] public required string EntityState { get; set; }
}
