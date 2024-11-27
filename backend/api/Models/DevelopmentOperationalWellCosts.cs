using api.Models.Interfaces;

namespace api.Models;

public class DevelopmentOperationalWellCosts : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public double RigUpgrading { get; set; }
    public double RigMobDemob { get; set; }
    public double AnnualWellInterventionCostPerWell { get; set; }
    public double PluggingAndAbandonment { get; set; }
}
