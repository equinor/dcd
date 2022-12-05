namespace api.Models
{
    public class DevelopmentOperationalWellCosts
    {
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public double RigUpgrading { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCostPerWell { get; set; }
        public double PluggingAndAbandonment { get; set; }
    }
}
