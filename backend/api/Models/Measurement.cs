
namespace api.Models
{
    public abstract class Measurement
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
    }
    public class LengthMeasurement : Measurement
    {
        public LengthUnit Unit { get; set; }
    }

    public class WeightMeasurement : Measurement
    {
        public WeightUnit Unit { get; set; }
    }

    public enum LengthUnit
    {
        km
    }

    public enum WeightUnit
    {
        tonnes
    }
}
