using api.Models.Interfaces;

namespace api.Models;

public class RevisionDetails : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid RevisionId { get; set; }
    public Project Revision { get; set; } = null!;

    public string? RevisionName { get; set; }
    public bool Arena { get; set; }
    public bool Mdqc { get; set; }
    public ProjectClassification Classification { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
