using api.Models;

namespace api.Dtos
{
    public class SubstructureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public Guid SourceCaseId { get; set; }
        public SubstructureCostProfileDto CostProfile { get; set; } = null!;
        public double DryWeight { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class SubstructureCostProfileDto : TimeSeriesCost<double>
    {
    }
}
