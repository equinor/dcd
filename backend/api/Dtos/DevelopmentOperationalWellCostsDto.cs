namespace api.Dtos
{
    public class DevelopmentOperationalWellCostsDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public double RigUpgrading { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCostPerWell { get; set; }
        public double PluggingAndAbandonment { get; set; }
        public bool HasChanges { get; set; }
    }
}
