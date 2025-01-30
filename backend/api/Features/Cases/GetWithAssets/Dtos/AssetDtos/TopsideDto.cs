using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class TopsideDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required double DryWeight { get; set; }
    [Required] public required double OilCapacity { get; set; }
    [Required] public required double GasCapacity { get; set; }
    [Required] public required double WaterInjectionCapacity { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required double FuelConsumption { get; set; }
    [Required] public required double FlaredGas { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required double CO2ShareOilProfile { get; set; }
    [Required] public required double CO2ShareGasProfile { get; set; }
    [Required] public required double CO2ShareWaterInjectionProfile { get; set; }
    [Required] public required double CO2OnMaxOilProfile { get; set; }
    [Required] public required double CO2OnMaxGasProfile { get; set; }
    [Required] public required double CO2OnMaxWaterInjectionProfile { get; set; }
    [Required] public required int CostYear { get; set; }
    public required DateTime? ProspVersion { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    [Required] public required Source Source { get; set; }
    [Required] public required string ApprovedBy { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
    [Required] public required double FacilityOpex { get; set; }
    [Required] public required double PeakElectricityImported { get; set; }
}
