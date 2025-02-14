using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class RevisionDetails : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid RevisionId { get; set; }
    public Project Revision { get; set; } = null!;

    public required string? RevisionName { get; set; }
    public required bool Arena { get; set; }
    public required bool Mdqc { get; set; }
    public required ProjectClassification Classification { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
