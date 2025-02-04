using api.Models.Interfaces;

namespace api.Models;

public class DevelopmentOperationalWellCosts : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public double RigUpgrading { get; set; }
    public double RigMobDemob { get; set; }
    public double AnnualWellInterventionCostPerWell { get; set; }
    public double PluggingAndAbandonment { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}
