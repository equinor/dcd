using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Transport
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
        public Transport Transport { get; set; } = null!;
    }

    public class GasExportPipelineLength : LengthMeasurement
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
    }

    public class OilExportPipelineLength : LengthMeasurement
    {
        [ForeignKey("Transport.Id")]
        public Transport Transport { get; set; } = null!;
    }
}
