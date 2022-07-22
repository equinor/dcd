using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Exploration
    {
        public Guid Id { get; set; }
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ExplorationCostProfile? CostProfile { get; set; }
        public GAndGAdminCost? GAndGAdminCost { get; set; }
        public double RigMobDemob { get; set; }
        public Currency Currency { get; set; }
        public ICollection<ExplorationWell>? ExplorationWells { get; set; }
    }

    public class ExplorationCostProfile : TimeSeriesCost
    {
        [ForeignKey("Exploration.Id")]
        public Exploration Exploration { get; set; } = null!;
        public bool Override {get; set; }
    }

    public class GAndGAdminCost : TimeSeriesCost
    {
        [ForeignKey("Exploration.Id")]
        public Exploration Exploration { get; set; } = null!;
    }
}
