using api.Models;

namespace api.Dtos
{

    public class ExplorationDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ExplorationCostProfileDto? CostProfile { get; set; }
        public ExplorationDrillingScheduleDto? DrillingSchedule { get; set; }
        public GAndGAdminCostDto? GAndGAdminCost { get; set; }
        public double RigMobDemob { get; set; }
        public Currency Currency { get; set; }
        public ExplorationWellType? ExplorationWellType { get; set; }
    }

    public class ExplorationCostProfileDto : TimeSeriesCostDto { }
    public class ExplorationDrillingScheduleDto : TimeSeriesScheduleDto { }
    public class GAndGAdminCostDto : TimeSeriesCostDto { }
}
