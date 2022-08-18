using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class WellProject
    {
        public Guid Id { get; set; }
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WellProjectCostProfile? CostProfile { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCost { get; set; }
        public double PluggingAndAbandonment { get; set; }
        public Currency Currency { get; set; }
        public ICollection<WellProjectWell>? WellProjectWells { get; set; }
    }

    public class WellProjectCostProfile : TimeSeriesCost
    {
        [ForeignKey("WellProject.Id")]
        public WellProject WellProject { get; set; } = null!;
        public bool Override { get; set; }
    }
}
