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
        public ExplorationDrillingSchedule? DrillingSchedule { get; set; }
        public GAndGAdminCost? GAndGAdminCost { get; set; }
        public double RigMobDemob { get; set; }
        public Currency Currency { get; set; }
    }

    public class ExplorationCostProfile : TimeSeriesCost
    {
        [ForeignKey("Exploration.Id")]
        public Exploration Exploration { get; set; } = null!;
    }

    public class ExplorationDrillingSchedule : TimeSeriesSchedule
    {
        [ForeignKey("Exploration.Id")]
        public Exploration Exploration { get; set; } = null!;
    }

    public class GAndGAdminCost : TimeSeriesCost
    {
        [ForeignKey("Exploration.Id")]
        public Exploration Exploration { get; set; } = null!;
    }
}
