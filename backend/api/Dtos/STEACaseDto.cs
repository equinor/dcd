
namespace api.Dtos
{

    public class STEACaseDto
    {
        public STEACaseDto()
        {

        }
        public string Name { get; set; } = null!;
        public ExplorationCostProfileDto Exploration { get; set; } = null!;

        public CapexDto Capex { get; set; } = null!;

        public ProductionAndSalesVolumesDto ProductionAndSalesVolumes { get; set; } = null!;
    }

    public class CapexDto
    {
        public CapexDto()
        {

        }
        public WellProjectCostProfileDto Drilling { get; set; } = null!;

        public OffshoreFacilitiesCostProfileDto OffshoreFacilities { get; set; } = null!;
    }

    public class ProductionAndSalesVolumesDto
    {
        public ProductionProfileOilDto TotalAndAnnualOil { get; set; } = null!;
        public NetSalesGasDto TotalAndAnnualSalesGas { get; set; } = null!;
        public Co2EmissionsDto Co2Emissions { get; set; } = null!;
    }

    public class OffshoreFacilitiesCostProfileDto : TimeSeriesCostDto
    {

    }

    public class TotalAndAnnualOil : ProductionProfileOilDto
    {

    }
    public class TotalAndAnnualSalesGas : NetSalesGasDto
    {

    }
}
