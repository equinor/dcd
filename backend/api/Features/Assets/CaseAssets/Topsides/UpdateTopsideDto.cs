using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideDto
{
    public double DryWeight { get; set; }
    public double OilCapacity { get; set; }
    public double GasCapacity { get; set; }
    public double WaterInjectionCapacity { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public double FuelConsumption { get; set; }
    public double FlaredGas { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double CO2ShareOilProfile { get; set; }
    public double CO2ShareGasProfile { get; set; }
    public double CO2ShareWaterInjectionProfile { get; set; }
    public double CO2OnMaxOilProfile { get; set; }
    public double CO2OnMaxGasProfile { get; set; }
    public double CO2OnMaxWaterInjectionProfile { get; set; }
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public double FacilityOpex { get; set; }
    public double PeakElectricityImported { get; set; }
    public Source Source { get; set; }
    public Maturity Maturity { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public class ProspUpdateTopsideDto
{
    public double DryWeight { get; set; }
    public double OilCapacity { get; set; }
    public double GasCapacity { get; set; }
    public double WaterInjectionCapacity { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public double FuelConsumption { get; set; }
    public double FlaredGas { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double CO2ShareOilProfile { get; set; }
    public double CO2ShareGasProfile { get; set; }
    public double CO2ShareWaterInjectionProfile { get; set; }
    public double CO2OnMaxOilProfile { get; set; }
    public double CO2OnMaxGasProfile { get; set; }
    public double CO2OnMaxWaterInjectionProfile { get; set; }
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public double FacilityOpex { get; set; }
    public double PeakElectricityImported { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
}
