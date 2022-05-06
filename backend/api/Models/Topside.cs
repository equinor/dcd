using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Topside
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Project Project { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public TopsideCostProfile? CostProfile { get; set; }
        public TopsideCessationCostProfile? CessationCostProfile { get; set; }
        public double DryWeight { get; set; }
        public double OilCapacity { get; set; }
        public double GasCapacity { get; set; }
        public double FacilitiesAvailability { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public Maturity Maturity { get; set; }
    }

    public class TopsideCostProfile : TimeSeriesCost
    {
        [ForeignKey("Topside.Id")]
        public Topside Topside { get; set; } = null!;
    }

    public class TopsideCessationCostProfile : TimeSeriesCost
    {
        [ForeignKey("Topside.Id")]
        public Topside Topside { get; set; } = null!;
    }
}
