using api.Models;

namespace api.Dtos
{
    public class WellDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public WellTypeDto? WellType { get; set; }
        public ExplorationWellTypeDto? ExplorationWellType { get; set; }
        public double WellInterventionCost { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
    }

    public class WellTypeDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public WellTypeCategoryDto Category { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
        public string? Description { get; set; }
    }

    public enum WellTypeCategoryDto
    {
        Oil_Producer,
        Gas_Producer,
        Water_Injector,
        Gas_Injector,
        Exploration_Well,
        Appraisal_Well,
        Sidetrack
    }

    public class ExplorationWellTypeDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ExplorationWellTypeCategoryDto Category { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
        public string? Description { get; set; }
    }

    public enum ExplorationWellTypeCategoryDto
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
