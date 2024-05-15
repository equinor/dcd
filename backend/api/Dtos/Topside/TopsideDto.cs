using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class TopsideDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty!;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public double DryWeight { get; set; }
    [Required]
    public double OilCapacity { get; set; }
    [Required]
    public double GasCapacity { get; set; }
    [Required]
    public double WaterInjectionCapacity { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public Maturity Maturity { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public double FuelConsumption { get; set; }
    [Required]
    public double FlaredGas { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public double CO2ShareOilProfile { get; set; }
    [Required]
    public double CO2ShareGasProfile { get; set; }
    [Required]
    public double CO2ShareWaterInjectionProfile { get; set; }
    [Required]
    public double CO2OnMaxOilProfile { get; set; }
    [Required]
    public double CO2OnMaxGasProfile { get; set; }
    [Required]
    public double CO2OnMaxWaterInjectionProfile { get; set; }
    [Required]
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    [Required]
    public Source Source { get; set; }
    [Required]
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    [Required]
    public double FacilityOpex { get; set; }
    [Required]
    public double PeakElectricityImported { get; set; }
}
