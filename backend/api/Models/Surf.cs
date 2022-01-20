using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Surf
    {
        public Guid Id { get; set; }
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
        public virtual Surf Surf { get; set; } = null!;
    }

    public class InfieldPipelineSystemLength : Measurement
    {
        [ForeignKey("Surf.Id")]
        public Surf Surf { get; set; } = null!;
        public LengthUnit Unit { get; set; }
    }

    public class UmbilicalSystemLength : Measurement
    {
        [ForeignKey("Surf.Id")]
        public Surf Surf { get; set; } = null!;
        public LengthUnit Unit { get; set; }
    }

    public enum ProductionFlowline
    {
        Default = 999
    }
}
