using api.Models;

namespace api.Dtos
{

    public class ExplorationDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid SourceCaseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WellType WellType { get; set; }
        public ExplorationCostProfileDto? CostProfile { get; set; }
        public ExplorationDrillingScheduleDto? DrillingSchedule { get; set; }
        public GAndGAdminCostDto? GAndGAdminCost { get; set; }
        public double RigMobDemob { get; set; }
    }

    public class ExplorationCostProfileDto : TimeSeriesCost<double> { }
    public class ExplorationDrillingScheduleDto : TimeSeriesSchedule { }
    public class GAndGAdminCostDto : TimeSeriesCost<double> { }
}
