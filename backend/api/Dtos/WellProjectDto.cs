using api.Models;

namespace api.Dtos
{
    public class WellProjectDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WellProjectCostProfileDto? CostProfile { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public double RigMobDemob { get; set; }
        public double AnnualWellInterventionCost { get; set; }
        public double PluggingAndAbandonment { get; set; }
        public Currency Currency { get; set; }
        public List<WellProjectWellDto>? WellProjectWells { get; set; }
    }

    public class WellProjectCostProfileDto : TimeSeriesCostDto
    {
        public bool Override { get; set; }
    }
}
