using api.Models.Interfaces;

namespace api.Models;

public class Image : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public Guid? CaseId { get; set; }
    public virtual Case? Case { get; set; }
}
