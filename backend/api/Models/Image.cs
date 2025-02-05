using api.Models.Interfaces;

namespace api.Models;

public class Image : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public required string Url { get; set; }
    public required string? Description { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid? CaseId { get; set; }
    public Case? Case { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
