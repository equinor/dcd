using api.Models.Interfaces;

namespace api.Models
{
    public class ExplorationOperationalWellCosts : IHasProjectId
    {
        public virtual Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public double ExplorationRigUpgrading { get; set; }
        public double ExplorationRigMobDemob { get; set; }
        public double ExplorationProjectDrillingCosts { get; set; }
        public double AppraisalRigMobDemob { get; set; }
        public double AppraisalProjectDrillingCosts { get; set; }
    }
}
