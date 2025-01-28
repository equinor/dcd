using api.Features.Profiles.Dtos;

namespace api.Features.Stea.Dtos;

public class SteaCaseDto
{
    public string Name { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public TimeSeriesCostDto Exploration { get; set; } = new();
    public CapexDto Capex { get; set; } = new();
    public ProductionAndSalesVolumesDto ProductionAndSalesVolumes { get; set; } = new();
    public TimeSeriesCostDto StudyCostProfile { get; set; } = new();
    public TimeSeriesCostDto OpexCostProfile { get; set; } = new();
}

public class CapexDto
{
    public TimeSeriesCostDto Summary { get; set; } = new();
    public TimeSeriesCostDto Drilling { get; set; } = new();
    public TimeSeriesCostDto OffshoreFacilities { get; set; } = new();
    public TimeSeriesCostDto CessationCost { get; set; } = new();
    public TimeSeriesCostDto OnshorePowerSupplyCost { get; set; } = new();
}

public class ProductionAndSalesVolumesDto
{
    public int StartYear { get; set; }
    public TimeSeriesCostDto TotalAndAnnualOil { get; set; } = new();
    public TimeSeriesCostDto TotalAndAnnualSalesGas { get; set; } = new();
    public TimeSeriesCostDto Co2Emissions { get; set; } = new();
    public TimeSeriesCostDto ImportedElectricity { get; set; } = new();
    public TimeSeriesCostDto AdditionalOil { get; set; } = new();
    public TimeSeriesCostDto AdditionalGas { get; set; } = new();
}
