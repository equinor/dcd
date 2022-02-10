using api.Models;

namespace api.Dtos
{
    public class WellProjectDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WellProjectCostProfileDto CostProfile { get; set; } = null!;
        public DrillingScheduleDto DrillingSchedule { get; set; } = null!;
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCost { get; set; }
        public double PluggingAndAbandonment { get; set; }
    }

    public class WellProjectCostProfileDto : TimeSeriesCost
    {
    }
    public class DrillingScheduleDto : TimeSeriesSchedule
    {
    }
}
