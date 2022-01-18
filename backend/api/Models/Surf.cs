using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Surf
    {
        public Guid Id { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual SurfCostProfile SurfCostProfile { get; set; } = null!;
        public Maturity Maturity { get; set; }
        public virtual SurfMeasurement InfieldPipelineSystemLength { get; set; } = null!;
        public virtual SurfMeasurement UmbilicalSystemLength { get; set; } = null!;

    }

    public enum ProductionFlowline
    {
      StartPlaceholder,
      StopPlaceholder
    }

    public enum Maturity
    {
        A,
        B,
        C,
        D
    }
}

