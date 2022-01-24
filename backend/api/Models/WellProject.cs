using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace api.Models
{
    public class WellProject
    {
        public Guid Id { get; set; }
        public Project Project { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public WellType WellType { get; set; }
        public WellProjectCostProfile CostProfile { get; set; } = null!;
        public DrillingSchedule DrillingSchedule { get; set; } = null!;
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCost { get; set; }
        public double PluggingAndAbandonment { get; set; }
    }

    public enum WellType
    {
        Oil,
        Gas
    }

    public class WellProjectCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("WellProject.Id")]
        public WellProject WellProject { get; set; } = null!;
    }
    public class DrillingSchedule : TimeSeriesBase<int>
    {
        [ForeignKey("WellProject.Id")]
        public WellProject WellProject { get; set; } = null!;
    }
}
