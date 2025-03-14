using System.ComponentModel.DataAnnotations;

namespace api.Features.ChangeLogs;

public class ProjectChangeLogDto
{
    public required string? EntityDescription { get; set; }
    [Required] public required Guid EntityId { get; set; }
    [Required] public required string EntityName { get; set; }
    public required string? PropertyName { get; set; }
    public required string? OldValue { get; set; }
    public required string? NewValue { get; set; }
    public required string? Username { get; set; }
    [Required] public required DateTime TimestampUtc { get; set; }
    [Required] public required string EntityState { get; set; }
    [Required] public required ChangeLogCategory Category { get; set; }
}

public enum ChangeLogCategory
{
    None,
    WellCostTab,
    Co2Tab,
    AccessManagementTab,
    SettingsTab,
    ProjectOverviewTab
}
