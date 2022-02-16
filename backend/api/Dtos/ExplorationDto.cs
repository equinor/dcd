using api.Models;

namespace api.Dtos
{

    public class ExplorationDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WellType WellType { get; set; }
        public ExplorationCostProfileDto CostProfile { get; set; } = null!;
        public ExplorationDrillingScheduleDto DrillingSchedule { get; set; } = null!;
        public GAndGAdminCostDto GAndGAdminCost { get; set; } = null!;
        public double RigMobDemob { get; set; }
    }

    public class ExplorationCostProfileDto : TimeSeriesCostDto { }
    public class ExplorationDrillingScheduleDto : TimeSeriesScheduleDto { }
    public class GAndGAdminCostDto : TimeSeriesCostDto { }
}
