using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class OnshorePowerSupply : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public required string Name { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
}
