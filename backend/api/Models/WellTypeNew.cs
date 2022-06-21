namespace api.Models
{
    public class WellTypeNew
    {
        public WellTypeName Name { get; set; }
        public double WellCost { get; set; }
        public double DrillingDays { get; set; }
    }

    public enum WellTypeName
    {
        Oil_Producer,
        Gas_Producer,
        Water_Injector,
        Gas_Injector,
        Exploration_Well,
    }
}
