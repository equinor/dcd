using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Profiles.Dtos;

public class TopsideWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public TopsideCostProfileDto CostProfile { get; set; } = new();
    [Required]
    public TopsideCostProfileOverrideDto CostProfileOverride { get; set; } = new();
    [Required]
    public TopsideCessationCostProfileDto CessationCostProfile { get; set; } = new();
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
    public DateTime? ProspVersion { get; set; }
    public DateTime? LastChangedDate { get; set; }
    [Required]
    public Source Source { get; set; }
    [Required]
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    [Required]
    public double FacilityOpex { get; set; }
    [Required]
    public double PeakElectricityImported { get; set; }
}

public class TopsideCostProfileDto : TimeSeriesCostDto;

public class TopsideCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class TopsideCessationCostProfileDto : TimeSeriesCostDto;
