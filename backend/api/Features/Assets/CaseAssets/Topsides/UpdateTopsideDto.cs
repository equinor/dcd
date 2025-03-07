using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Topsides;

public class UpdateTopsideDto
{
    [Required] public required double DryWeight { get; set; }
    [Required] public required double OilCapacity { get; set; }
    [Required] public required double GasCapacity { get; set; }
    [Required] public required double WaterInjectionCapacity { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required double FuelConsumption { get; set; }
    [Required] public required double FlaredGas { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required double Co2ShareOilProfile { get; set; }
    [Required] public required double Co2ShareGasProfile { get; set; }
    [Required] public required double Co2ShareWaterInjectionProfile { get; set; }
    [Required] public required double Co2OnMaxOilProfile { get; set; }
    [Required] public required double Co2OnMaxGasProfile { get; set; }
    [Required] public required double Co2OnMaxWaterInjectionProfile { get; set; }
    [Required] public required int CostYear { get; set; }
    [Required] public required double FacilityOpex { get; set; }
    [Required] public required double PeakElectricityImported { get; set; }
    [Required] public required Source Source { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required string ApprovedBy { get; set; }
}
