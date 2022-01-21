using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Transport : IFacility
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public TransportCostProfile CostProfile { get; set; } = null!;
        public GasExportPipelineLength GasExportPipelineLength { get; set; } = null!;
        public OilExportPipelineLength OilExportPipelineLength { get; set; } = null!;
        public Maturity Maturity { get; set; }
    }

    public class TransportCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Transport.Id")]
        public virtual Transport Transport { get; set; } = null!;
    }

    public class GasExportPipelineLength : Measurement
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
        public LengthUnit Unit { get; set; }
    }

    public class OilExportPipelineLength : Measurement
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
        public LengthUnit Unit { get; set; }
    }
}
