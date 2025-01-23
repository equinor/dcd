
using api.Models.Interfaces;

namespace api.Models;

public class Well : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string? Name { get; set; }
    public WellCategory WellCategory { get; set; }
    public double WellCost { get; set; }
    public double DrillingDays { get; set; }
    public double PlugingAndAbandonmentCost { get; set; }
    public double WellInterventionCost { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual ICollection<WellProjectWell> WellProjectWells { get; set; } = [];
    public virtual ICollection<ExplorationWell> ExplorationWells { get; set; } = [];
}

public enum WellCategory
{
    Oil_Producer,
    Gas_Producer,
    Water_Injector,
    Gas_Injector,
    Exploration_Well,
    Appraisal_Well,
    Sidetrack,
    RigMobDemob,
}
