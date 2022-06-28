using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Well
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public WellType? WellType { get; set; }
        public ExplorationWellType? ExplorationWellType { get; set; }
        public double WellInterventionCost { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
    }


    public class WellType
    {
        [ForeignKey("Well.Id")]
        public Well Well { get; set; } = null!;
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public WellTypeCategory Category { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
        public string? Description { get; set; }
    }

    public enum WellTypeCategory
    {
        Oil_Producer,
        Gas_Producer,
        Water_Injector,
        Gas_Injector,
        Exploration_Well,
        Appraisal_Well,
        Sidetrack
    }

    public class ExplorationWellType
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ExplorationWellTypeCategory Category { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
        public string? Description { get; set; }
    }

    public enum ExplorationWellTypeCategory
    {
        Oil_Producer,
        Gas_Producer,
        Water_Injector,
        Gas_Injector,
        Exploration_Well,
        Appraisal_Well,
        Sidetrack
    }
}
