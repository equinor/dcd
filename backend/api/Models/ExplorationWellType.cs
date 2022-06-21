namespace api.Models
{
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
