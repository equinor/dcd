
namespace api.Models;

public class Well
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string? Name { get; set; }
    public WellCategory WellCategory { get; set; }
    public double WellCost { get; set; }
    public double DrillingDays { get; set; }
    public double PlugingAndAbandonmentCost { get; set; }
    public double WellInterventionCost { get; set; }
    public ICollection<WellProjectWell>? WellProjectWells { get; set; }
    public ICollection<ExplorationWell>? ExplorationWells { get; set; }
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
