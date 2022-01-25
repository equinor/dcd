using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Surf
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public virtual Project Project { get; set; } = null!;
        public virtual SurfCostProfile CostProfile { get; set; } = null!;
        public Maturity Maturity { get; set; }
        public virtual InfieldPipelineSystemLength InfieldPipelineSystemLength { get; set; } = null!;
        public virtual UmbilicalSystemLength UmbilicalSystemLength { get; set; } = null!;
        public ProductionFlowline ProductionFlowline { get; set; }
    }

    public class SurfCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Surf.Id")]
        public virtual Surf Surf { get; set; } = null!;
    }

    public class InfieldPipelineSystemLength : LengthMeasurement
    {
        [ForeignKey("Surf.Id")]
        public virtual Surf Surf { get; set; } = null!;
    }

    public class UmbilicalSystemLength : LengthMeasurement
    {
        [ForeignKey("Surf.Id")]
        public virtual Surf Surf { get; set; } = null!;
    }

    public enum ProductionFlowline
    {
        Default = 999
    }
}
