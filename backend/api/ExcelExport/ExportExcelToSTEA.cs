
using api.Dtos;

using ClosedXML.Excel;

namespace api.Excel;

public static class ExportToSTEA
{
    public static List<BusinessCase> Export(STEAProjectDto project)
    {
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Input to STEA");
        ws.Cell("B2").Value = project.Name;
        int rowCount = 3;
        var businessCases = new List<BusinessCase>();
        foreach (STEACaseDto c in project.STEACases)
        {
            var businessCase = new BusinessCase();
            int headerRowCount = rowCount;
            rowCount++;
            businessCase.Exploration = CreateExcelRow("Exploration Cost [Expected Real MNOK'21]", project.StartYear, c.Exploration, rowCount, 1);

            rowCount++;
            businessCase.Capex = CreateExcelRow("Capex [Expected Real MNOK'21]", project.StartYear, c.Capex, rowCount, 1);

            rowCount++;
            businessCase.Drilling = CreateExcelRow("Drilling", project.StartYear, c.Capex.Drilling, rowCount, 1);

            rowCount++;
            businessCase.OffshoreFacilites = CreateExcelRow("Offshore Facilities", project.StartYear, c.Capex.OffshoreFacilities, rowCount, 1);

            rowCount++;
            businessCase.StudyCost = CreateExcelRow("Study cost", project.StartYear, c.StudyCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Opex = CreateExcelRow("Opex", project.StartYear, c.OpexCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Cessation = CreateExcelRow("Cessation - Offshore Facilities", project.StartYear, c.Capex.CessationCost, rowCount, 1);

            rowCount++;
            businessCase.ProductionAndSalesVolumes = new ExcelTableCell(ColumnNumber(1) + rowCount.ToString(), "Production And Sales Volumes");

            rowCount++;
            businessCase.TotalAndAnnualOil = CreateExcelRow("Total And annual Oil/Condensate production [MSm3]", project.StartYear,
                c.ProductionAndSalesVolumes.TotalAndAnnualOil, rowCount, 1);

            rowCount++;
            businessCase.NetSalesGas = CreateExcelRow("Net Sales Gas [GSm3]", project.StartYear,
                c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas, rowCount, 1);

            rowCount++;
            businessCase.Co2Emissions = CreateExcelRow("Co2 Emissions [mill tonnes]", project.StartYear,
                c.ProductionAndSalesVolumes.Co2Emissions, rowCount, 1);

            rowCount++;
            businessCase.ImportedElectricity = CreateExcelRow("Imported electricity [GWh]", project.StartYear,
                c.ProductionAndSalesVolumes.ImportedElectricity, rowCount, 1);

            rowCount += 2;
            var allRows = new List<int>();
            if (c.Exploration.Values != null)
            {
                allRows.Add(c.Exploration.Values.Length + c.Exploration.StartYear);
            }
            if (c.Capex.Values != null)
            {
                allRows.Add(c.Capex.Values.Length + c.Capex.StartYear);
            }
            if (c.Capex.CessationCost.Values != null)
            {
                allRows.Add(c.Capex.CessationCost.Values.Length + c.Capex.CessationCost.StartYear);
            }
            if (c.OpexCostProfile.Values != null)
            {
                allRows.Add(c.OpexCostProfile.Values.Length + c.OpexCostProfile.StartYear);
            }
            if (c.StudyCostProfile.Values != null)
            {
                allRows.Add(c.StudyCostProfile.Values.Length + c.StudyCostProfile.StartYear);
            }
            if (c.ProductionAndSalesVolumes.TotalAndAnnualOil.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.TotalAndAnnualOil.Values.Length + c.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear);
            }
            if (c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values.Length + c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
            }
            if (c.ProductionAndSalesVolumes.Co2Emissions.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.Co2Emissions.Values.Length + c.ProductionAndSalesVolumes.Co2Emissions.StartYear);
            }

            int maxYear = allRows.Count > 0 ? allRows.Max() - project.StartYear : 0;

            businessCase.Header = CreateTableHeader(project.StartYear, project.StartYear + maxYear - 1, c.Name, headerRowCount);
            businessCases.Add(businessCase);
        }
        return businessCases;
    }

    private static void ValueToCells(List<ExcelTableCell> tableCells, int columnCount, int rowCount, TimeSeriesDto<double> e, int ProjectStartYear, double factor)
    {
        if (e.Values == null)
        {
            return;
        }
        columnCount += e.StartYear - ProjectStartYear;
        columnCount++;
        for (int i = 0; i < e.Values.Length; i++)
        {
            string cellNo = ColumnNumber(columnCount++) + rowCount.ToString();
            tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Values[i]).ToString()));
        }
    }

    private static List<ExcelTableCell> CreateExcelRow(string title, int ProjectStartYear, TimeSeriesDoubleDto e, int rowCount, double factor)
    {
        var tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = ColumnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, title));

        cellNo = ColumnNumber(columnCount) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Sum).ToString()));
        ValueToCells(tableCells, columnCount, rowCount, e, ProjectStartYear, factor);

        return tableCells;
    }

    private static List<ExcelTableCell> CreateTableHeader(int StartYear, int EndYear, string caseName, int rowCount)
    {
        var tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = ColumnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, caseName));
        cellNo = ColumnNumber(columnCount++) + rowCount;
        tableCells.Add(new ExcelTableCell(cellNo, "Total Cost"));
        for (int i = StartYear; i <= EndYear; i++)
        {
            cellNo = ColumnNumber(columnCount++) + rowCount.ToString();
            tableCells.Add(new ExcelTableCell(cellNo, i.ToString()));
        }
        return tableCells;
    }

    private static string ColumnNumber(int cellNumber)
    {
        int rest = cellNumber % 26;
        string rv = ((char)(rest + 65)).ToString();
        int newCellNumber = (cellNumber - rest) / 26;
        while (newCellNumber > 0)
        {
            rest = newCellNumber % 26;
            rv = ((char)(rest + 64)).ToString() + rv;
            newCellNumber = (newCellNumber - rest) / 26;
        }

        return rv;
    }
}
public class BusinessCase
{
    public List<ExcelTableCell> Header { get; set; }
    public List<ExcelTableCell> Exploration { get; set; }
    public List<ExcelTableCell> Capex { get; set; }

    public List<ExcelTableCell> OffshoreFacilites { get; set; }
    public List<ExcelTableCell> Cessation { get; set; }
    public List<ExcelTableCell> Drilling { get; set; }

    public ExcelTableCell ProductionAndSalesVolumes { get; set; } = null!;

    public List<ExcelTableCell> TotalAndAnnualOil { get; set; }

    public List<ExcelTableCell> NetSalesGas { get; set; }

    public List<ExcelTableCell> Co2Emissions { get; set; }

    public List<ExcelTableCell> StudyCost { get; set; }

    public List<ExcelTableCell> Opex { get; set; }

    public List<ExcelTableCell> ImportedElectricity { get; set; }
    public BusinessCase()
    {
        Header = new List<ExcelTableCell>();
        Exploration = new List<ExcelTableCell>();
        Capex = new List<ExcelTableCell>();
        OffshoreFacilites = new List<ExcelTableCell>();
        Cessation = new List<ExcelTableCell>();
        Drilling = new List<ExcelTableCell>();
        TotalAndAnnualOil = new List<ExcelTableCell>();
        NetSalesGas = new List<ExcelTableCell>();
        Co2Emissions = new List<ExcelTableCell>();
        StudyCost = new List<ExcelTableCell>();
        Opex = new List<ExcelTableCell>();
        ImportedElectricity = new List<ExcelTableCell>();
    }
}

public class ExcelTableCell
{
    public string CellNo { get; set; }
    public string Value { get; set; }
    public ExcelTableCell(string CellNo, string Value)
    {
        this.CellNo = CellNo;
        this.Value = Value;
    }
}

