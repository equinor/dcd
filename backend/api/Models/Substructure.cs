using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Substructure
    {
        public Guid Id { get; set; }
        public Project Project { get; set; } = null!;
        public SubstructureCostProfile CostProfile { get; set; } = null!;
        public SubstructureDryWeight DryWeight { get; set; } = null!;
        public Maturity Maturity { get; set; }
    }

    public class SubstructureCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Substructure.Id")]
        public Substructure Substructure { get; set; } = null!;
    }

    public class SubstructureDryWeight : Measurement
    {
        [ForeignKey("Substructure.Id")]
        public Substructure Substructure { get; set; } = null!;
        public WeightUnit Unit { get; set; }
    }
}
