namespace api.Models
{
    public class ExplorationOperationalWellCosts
    {
        public Project? Project { get; set; }
        public Guid Id { get; set; }
        public double ExplorationRigUpgrading { get; set; }
        public double ExplorationRigMobDemob { get; set; }
        public double ExplorationProjectDrillingCosts { get; set; }
        public double AppraisalRigMobDemob { get; set; }
        public double AppraisalProjectDrillingCosts { get; set; }
    }
}
