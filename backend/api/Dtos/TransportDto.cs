using api.Models;
namespace api.Dtos
{
    public class TransportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public TransportCostProfileDto CostProfile { get; set; } = null!;
        public Maturity Maturity { get; set; }
        public double GasExportPipelineLength { get; set; }
        public double OilExportPipelineLength { get; set; }
    }

    public class TransportCostProfileDto : TimeSeriesCost<double>
    {

    }
}
