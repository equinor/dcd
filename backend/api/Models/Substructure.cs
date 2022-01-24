using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Substructure
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public virtual Project Project { get; set; } = null!;
        public virtual SubstructureCostProfile CostProfile { get; set; } = null!;
        public virtual SubstructureDryWeight DryWeight { get; set; } = null!;
        public Maturity Maturity { get; set; }
    }

    public class SubstructureCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Substructure.Id")]
        public virtual Substructure Substructure { get; set; } = null!;
    }

    public class SubstructureDryWeight : Measurement
    {
        [ForeignKey("Substructure.Id")]
        public virtual Substructure Substructure { get; set; } = null!;
        public WeightUnit Unit { get; set; }
    }
}
