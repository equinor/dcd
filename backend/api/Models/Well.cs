
namespace api.Models
{
    public class Well
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public WellTypeCategory WellTypeCategory { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
        public double PlugingAndAbandonmentCost { get; set; }
        public double RigMobDemob { get; set; }
        public double WellInterventionCost { get; set; }
        public ICollection<WellCase>? WellCases { get; set; }
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
