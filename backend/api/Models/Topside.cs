using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Topside
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public TopsideCostProfile CostProfile { get; set; } = null!;
        public double DryWeight { get; set; }
        public double OilCapacity { get; set; }
        public double GasCapacity { get; set; }
        public double FacilitiesAvailability { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class TopsideCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Topside.Id")]
        public Topside Topside { get; set; } = null!;
    }
}
