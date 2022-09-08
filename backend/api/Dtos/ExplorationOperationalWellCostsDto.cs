namespace api.Dtos
{
    public class ExplorationOperationalWellCostsDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public double RigUpgrading { get; set; }
        public double ExplorationRigMobDemob { get; set; }
        public double ExplorationProjectDrillingCosts { get; set; }
        public double AppraisalRigMobDemob { get; set; }
        public double AppraisalProjectDrillingCosts { get; set; }
    }
}
