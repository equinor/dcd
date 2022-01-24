using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Topside
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public virtual Project Project { get; set; } = null!;
        public virtual TopsideCostProfile CostProfile { get; set; } = null!;
        public virtual TopsideDryWeight DryWeight { get; set; } = null!;
        public double OilCapacity { get; set; }
        public double GasCapacity { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class TopsideCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Topside.Id")]
        public virtual Topside Topside { get; set; } = null!;
    }

    public class TopsideDryWeight : Measurement
    {
        [ForeignKey("Topside.Id")]
        public virtual Topside Topside { get; set; } = null!;
        public WeightUnit Unit { get; set; }
    }
}
