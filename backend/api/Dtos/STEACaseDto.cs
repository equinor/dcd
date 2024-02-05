
namespace api.Dtos;

public class STEACaseDto
{
    public string Name { get; set; } = string.Empty;
    public int StartYear { get; set; }
    public TimeSeriesCostDto Exploration { get; set; } = new();

    public CapexDto Capex { get; set; } = new();

    public ProductionAndSalesVolumesDto ProductionAndSalesVolumes { get; set; } = new();
    public OffshoreFacilitiesCostProfileDto OffshoreFacilitiesCostProfileDto { get; set; } = new();

    public StudyCostProfileDto StudyCostProfile { get; set; } = new();
    public OpexCostProfileDto OpexCostProfile { get; set; } = new();
}

public class CapexDto : TimeSeriesCostDto
{
    public TimeSeriesCostDto Drilling { get; set; } = new();

    public OffshoreFacilitiesCostProfileDto OffshoreFacilities { get; set; } = new();
    public CessationCostDto CessationCost { get; set; } = new();
}

public class ProductionAndSalesVolumesDto
{
    public int StartYear { get; set; }
    public ProductionProfileOilDto TotalAndAnnualOil { get; set; } = new();
    public NetSalesGasDto TotalAndAnnualSalesGas { get; set; } = new();
    public Co2EmissionsDto Co2Emissions { get; set; } = new();
    public ImportedElectricityDto ImportedElectricity { get; set; } = new();
}

public class OffshoreFacilitiesCostProfileDto : TimeSeriesCostDto
{
}
