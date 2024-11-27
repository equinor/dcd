using api.Models.Interfaces;

namespace api.Models;

public class ExplorationOperationalWellCosts : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public double ExplorationRigUpgrading { get; set; }
    public double ExplorationRigMobDemob { get; set; }
    public double ExplorationProjectDrillingCosts { get; set; }
    public double AppraisalRigMobDemob { get; set; }
    public double AppraisalProjectDrillingCosts { get; set; }
}
