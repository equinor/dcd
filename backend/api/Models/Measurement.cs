using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public abstract class Measurement
    {
        public Guid Id { get; set; }
        public Unit Unit { get; set; }
        public double Value { get; set; }
    }

    public class SurfMeasurement
    {
        public Guid Id { get; set; }
        public Unit Unit { get; set; }
        public double Value { get; set; }
        [ForeignKey("Surf.Id")]
        public virtual Surf Surf { get; set; } = null!;
    }

    public enum Unit
    {
        Sm3,
        bbl,
        scf,
        J,
        Wh,
        kg,
        tonnes
    }
}
