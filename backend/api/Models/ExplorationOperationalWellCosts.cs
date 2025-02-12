using api.Models.Interfaces;

namespace api.Models;

public class ExplorationOperationalWellCosts : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required double ExplorationRigUpgrading { get; set; }
    public required double ExplorationRigMobDemob { get; set; }
    public required double ExplorationProjectDrillingCosts { get; set; }
    public required double AppraisalRigMobDemob { get; set; }
    public required double AppraisalProjectDrillingCosts { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
