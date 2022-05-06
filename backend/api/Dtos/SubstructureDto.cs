using api.Models;

namespace api.Dtos
{
    public class SubstructureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public SubstructureCostProfileDto? CostProfile { get; set; }
        public SubstructureCessationCostProfileDto? CessationCostProfile { get; set; }
        public double DryWeight { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class SubstructureCostProfileDto : TimeSeriesCostDto
    {
    }

    public class SubstructureCessationCostProfileDto : TimeSeriesCostDto
    {
    }
}
