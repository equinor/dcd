using api.Features.Profiles.Dtos;

namespace api.Features.Stea.Dtos;

public class SteaCaseDto
{
    public string Name { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public TimeSeries Exploration { get; set; } = new();
    public CapexDto Capex { get; set; } = new();
    public ProductionAndSalesVolumesDto ProductionAndSalesVolumes { get; set; } = new();
    public TimeSeries StudyCostProfile { get; set; } = new();
    public TimeSeries OpexCostProfile { get; set; } = new();
}

public class CapexDto
{
    public TimeSeries Summary { get; set; } = new();
    public TimeSeries Drilling { get; set; } = new();
    public TimeSeries OffshoreFacilities { get; set; } = new();
    public TimeSeries CessationCost { get; set; } = new();
    public TimeSeries OnshorePowerSupplyCost { get; set; } = new();
}

public class ProductionAndSalesVolumesDto
{
    public int StartYear { get; set; }
    public TimeSeries TotalAndAnnualOil { get; set; } = new();
    public TimeSeries TotalAndAnnualSalesGas { get; set; } = new();
    public TimeSeries Co2Emissions { get; set; } = new();
    public TimeSeries ImportedElectricity { get; set; } = new();
    public TimeSeries AdditionalOil { get; set; } = new();
    public TimeSeries AdditionalGas { get; set; } = new();
}
