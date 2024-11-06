
using api.Models.Interfaces;

namespace api.Models;

public class Well : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }
    public virtual Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string? Name { get; set; }
    public WellCategory WellCategory { get; set; }
    public double WellCost { get; set; }
    public double DrillingDays { get; set; }
    public double PlugingAndAbandonmentCost { get; set; }
    public double WellInterventionCost { get; set; }
    public virtual ICollection<WellProjectWell>? WellProjectWells { get; set; }
    public virtual ICollection<ExplorationWell>? ExplorationWells { get; set; }

    public static bool IsWellProjectWell(WellCategory wellCategory) => new[] {
        WellCategory.Oil_Producer,
        WellCategory.Gas_Producer,
        WellCategory.Water_Injector,
        WellCategory.Gas_Injector
    }.Contains(wellCategory);
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
