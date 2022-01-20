namespace api.Models
{
    public abstract class Measurement
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
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
