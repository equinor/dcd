namespace api.Models
{
    public class WellTypeNew
    {
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
}
