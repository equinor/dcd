using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Surf
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public SurfCostProfile CostProfile { get; set; } = null!;
        public Maturity Maturity { get; set; }
        public InfieldPipelineSystemLength InfieldPipelineSystemLength { get; set; } = null!;
        public UmbilicalSystemLength UmbilicalSystemLength { get; set; } = null!;
        public ProductionFlowline ProductionFlowline { get; set; }
    }

    public class SurfCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Surf.Id")]
        public Surf Surf { get; set; } = null!;
    }

    public class InfieldPipelineSystemLength : LengthMeasurement
    {
        [ForeignKey("Surf.Id")]
        public Surf Surf { get; set; } = null!;
    }

    public class UmbilicalSystemLength : LengthMeasurement
    {
        [ForeignKey("Surf.Id")]
        public Surf Surf { get; set; } = null!;
    }

    public enum ProductionFlowline
    {
        Default = 999
    }
}
