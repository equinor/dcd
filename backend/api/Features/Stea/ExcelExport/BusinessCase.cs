namespace api.Features.Stea.ExcelExport;

public class BusinessCase
{
    public List<ExcelTableCell> Header { get; set; } = [];
    public List<ExcelTableCell> Exploration { get; set; } = [];
    public List<ExcelTableCell> Capex { get; set; } = [];
    public List<ExcelTableCell> OffshoreFacilities { get; set; } = [];
    public List<ExcelTableCell> OnshorePowerSupply { get; set; } = [];
    public List<ExcelTableCell> Cessation { get; set; } = [];
    public List<ExcelTableCell> NglProduction { get; set; } = [];
    public List<ExcelTableCell> Drilling { get; set; } = [];
    public ExcelTableCell ProductionAndSalesVolumes { get; set; } = null!;
    public List<ExcelTableCell> TotalAndAnnualOil { get; set; } = [];
    public List<ExcelTableCell> NetSalesGas { get; set; } = [];
    public List<ExcelTableCell> Co2Emissions { get; set; } = [];
    public List<ExcelTableCell> StudyCost { get; set; } = [];
    public List<ExcelTableCell> Opex { get; set; } = [];
    public List<ExcelTableCell> ImportedElectricity { get; set; } = [];
    public List<ExcelTableCell> TotalExportedVolumes { get; set; } = [];
}
