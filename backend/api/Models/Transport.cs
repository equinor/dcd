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
        public TransportCessationCostProfile? CessationCostProfile { get; set; }
        public double GasExportPipelineLength { get; set; }
        public double OilExportPipelineLength { get; set; }
        public Maturity Maturity { get; set; }
        public Currency Currency { get; set; }
        public DateTimeOffset? LastChangedDate { get; set; }
        public int CostYear { get; set; }
        public Source Source { get; set; }
        public DateTimeOffset? ProspVersion { get; set; }
    }

    public class TransportCostProfile : TimeSeriesCost
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
    }

    public class TransportCessationCostProfile : TimeSeriesCost
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
    }
}
