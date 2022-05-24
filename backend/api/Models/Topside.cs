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
        public Currency Currency { get; set; }
        public double FuelConsumption { get; set; }
        public double FlaredGas { get; set; }
        public double CO2ShareOilProfile { get; set; }
        public double CO2ShareGasProfile { get; set; }
        public double CO2ShareWaterInjectionProfile { get; set; }
        public double CO2OnMaxOilProfile { get; set; }
        public double CO2OnMaxGasProfile { get; set; }
        public double CO2OnMaxWaterInjectionProfile { get; set; }
        public DateTimeOffset CostYear { get; set; }
        public string ProspVersion { get; set; } = string.Empty!;
        public DateTimeOffset LastChanged { get; set; }
        public Source Source { get; set; }
    }

    public class TopsideCostProfile : TimeSeriesFacilityCost
    {
        [ForeignKey("Topside.Id")]
        public Topside Topside { get; set; } = null!;
    }

    public class TopsideCessationCostProfile : TimeSeriesFacilityCost
    {
        [ForeignKey("Topside.Id")]
        public Topside Topside { get; set; } = null!;
    }
}
