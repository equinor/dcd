using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Transport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public TransportCostProfile? CostProfile { get; set; }
        public double GasExportPipelineLength { get; set; }
        public double OilExportPipelineLength { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class TransportCostProfile : TimeSeriesCost
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
    }
}
