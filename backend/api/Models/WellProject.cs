using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class WellProject
    {
        public Guid Id { get; set; }
        public Project Project { get; set; } = null!;
        public string Name { get; set; } = string.Empty;

        public WellProjectCostProfile CostProfile { get; set; } = null!;
        public DrillingSchedule DrillingSchedule { get; set; } = null!;
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCost { get; set; }
        public double PluggingAndAbandonment { get; set; }
    }

    public class WellProjectCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("WellProject.Id")]
        public WellProject WellProject { get; set; } = null!;
    }
    public class DrillingSchedule : TimeSeriesSchedule
    {
        [ForeignKey("WellProject.Id")]
        public WellProject WellProject { get; set; } = null!;
    }
}
