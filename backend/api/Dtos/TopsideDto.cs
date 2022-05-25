using api.Models;

namespace api.Dtos
{
    public class TopsideDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public Guid ProjectId { get; set; }
        public TopsideCostProfileDto? CostProfile { get; set; }
        public TopsideCessationCostProfileDto? CessationCostProfile { get; set; }
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
        public int CostYear { get; set; }
        public DateTimeOffset? ProspVersion { get; set; }
        public DateTimeOffset LastChangedDate { get; set; }
        public Source Source { get; set; }
    }

    public class TopsideCostProfileDto : TimeSeriesCostDto
    {

    }

    public class TopsideCessationCostProfileDto : TimeSeriesCostDto
    {

    }
}
