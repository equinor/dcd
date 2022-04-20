
using api.Dtos;

using ClosedXML.Excel;

namespace api.Excel;

public static class ExportToSTEA
{
    public static List<BusinessCase> export(STEAProjectDto project)
    {
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Input to STEA");
        ws.Cell("B2").Value = project.Name;
        int rowCount = 3;
        List<BusinessCase> businessCases = new List<BusinessCase>();
        foreach (STEACaseDto c in project.STEACases)
        {
            BusinessCase businessCase = new BusinessCase();
            int headerRowCount = rowCount;
            rowCount++;
            businessCase.Exploration = CreateExcelCostRow("Exploration Cost [Expected Real MNOK'21]", project.StartYear, c.Exploration, rowCount, 1e-6);
            rowCount++;
            businessCase.Capex = CreateExcelCostRow("Capex [Expected Real MNOK'21]", project.StartYear, c.Capex, rowCount, 1e-6);
            rowCount++;
            businessCase.Drilling = CreateExcelCostRow("Drilling", project.StartYear, c.Capex.Drilling, rowCount, 1e-6);
            rowCount++;
            businessCase.OffshoreFacilites = CreateExcelCostRow("Offshore Facilities", project.StartYear, c.Capex.OffshoreFacilities, rowCount, 1e-6);
            rowCount++;
            businessCase.CessationOffshoreFacilites = CreateExcelCostRow("Cessation - Offshore Facilities", project.StartYear, c.Capex.CessationOffshoreFacilities, rowCount, 1e-6);
            rowCount++;
            businessCase.ProductionAndSalesVolumes = new ExcelTableCell(columnNumber(1) + rowCount.ToString(), "Production And Sales Volumes");
            rowCount++;
            businessCase.TotalAndAnnualOil = CreateExcelVolumeRow("Total And annual Oil/Condensate production [MSm3]", project.StartYear,
                c.ProductionAndSalesVolumes.TotalAndAnnualOil, rowCount, 1e-6);
            rowCount++;
            businessCase.NetSalesGas = CreateExcelVolumeRow("Net Sales Gas [GSm3]", project.StartYear,
                c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas, rowCount, 1e-9);
            rowCount++;
            businessCase.Co2Emissions = CreateExcelMassRow("Co2 Emissions [mill tonnes]", project.StartYear,
                c.ProductionAndSalesVolumes.Co2Emissions, rowCount, 1e-6);
            rowCount += 2;
            List<int> allRows = new List<int>();
            if (c.Exploration.Values != null)
            {
                allRows.Add(c.Exploration.Values.Count() + c.Exploration.StartYear);
            }
            if (c.Capex.Values != null)
            {
                allRows.Add(c.Capex.Values.Count() + c.Capex.StartYear);
            }
            if (c.ProductionAndSalesVolumes.TotalAndAnnualOil.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.TotalAndAnnualOil.Values.Count() + c.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear);
            }
            if (c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values.Count() + c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
            }
            if (c.ProductionAndSalesVolumes.Co2Emissions.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.Co2Emissions.Values.Count() + c.ProductionAndSalesVolumes.Co2Emissions.StartYear);
            }

            int maxYear = allRows.Count() > 0 ? allRows.Max() - project.StartYear : 0;

            businessCase.Header = CreateTableHeader(project.StartYear, project.StartYear + maxYear - 1, c.Name, headerRowCount);
            businessCases.Add(businessCase);
        }
        return businessCases;

    }

    private static List<ExcelTableCell> CreateExcelCostRow(string title, int ProjectStartYear, TimeSeriesCostDto e, int rowCount, double factor)
    {
        List<ExcelTableCell> tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = columnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, title));

        cellNo = columnNumber(columnCount) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Sum).ToString()));
        ValueToCells(tableCells, columnCount, rowCount, e, ProjectStartYear, factor);

        return tableCells;
    }

    private static void ValueToCells(List<ExcelTableCell> tableCells, int columnCount, int rowCount, TimeSeriesDto<double> e, int ProjectStartYear, double factor)
    {
        if (e.Values == null)
        {
            return;
        }
        columnCount += e.StartYear - ProjectStartYear;
        columnCount++;
        for (int i = 0; i < e.Values.Count(); i++)
        {
            string cellNo = columnNumber(columnCount++) + rowCount.ToString();
            tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Values[i]).ToString()));
        }
    }


    private static List<ExcelTableCell> CreateExcelVolumeRow(string title, int ProjectStartYear, TimeSeriesVolumeDto e, int rowCount, double factor)
    {
        List<ExcelTableCell> tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = columnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, title));

        cellNo = columnNumber(columnCount) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Sum).ToString()));
        ValueToCells(tableCells, columnCount, rowCount, e, ProjectStartYear, factor);

        return tableCells;
    }

    private static List<ExcelTableCell> CreateExcelMassRow(string title, int ProjectStartYear, TimeSeriesMassDto e, int rowCount, double factor)
    {
        List<ExcelTableCell> tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = columnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, title));

        cellNo = columnNumber(columnCount) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Sum).ToString()));
        Console.WriteLine("Add values to cells " + title);
        ValueToCells(tableCells, columnCount, rowCount, e, ProjectStartYear, factor);

        return tableCells;
    }

    private static List<ExcelTableCell> CreateTableHeader(int StartYear, int EndYear, string caseName, int rowCount)
    {
        List<ExcelTableCell> tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = columnNumber(columnCount++) + rowCount.ToString();
        tableCells.Add(new ExcelTableCell(cellNo, caseName));
        cellNo = (columnNumber(columnCount++) + rowCount).ToString();
        tableCells.Add(new ExcelTableCell(cellNo, "Total Cost"));
        for (int i = StartYear; i <= EndYear; i++)
        {
            cellNo = columnNumber(columnCount++) + rowCount.ToString();
            tableCells.Add(new ExcelTableCell(cellNo, i.ToString()));
        }
        return tableCells;
    }

    private static string columnNumber(int cellNumber)
    {

        string rv = "";
        int newCellNumber = cellNumber;
        int rest = cellNumber % 26;
        rv = ((Char)(rest + 65)).ToString();
        newCellNumber = (cellNumber - rest) / 26;
        while (newCellNumber > 0)
        {
            rest = newCellNumber % 26;
            rv = ((Char)(rest + 64)).ToString() + rv;
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
    public List<ExcelTableCell> CessationOffshoreFacilites { get; set; }
    public List<ExcelTableCell> Drilling { get; set; }

    public ExcelTableCell ProductionAndSalesVolumes { get; set; } = null!;

    public List<ExcelTableCell> TotalAndAnnualOil { get; set; }

    public List<ExcelTableCell> NetSalesGas { get; set; }

    public List<ExcelTableCell> Co2Emissions { get; set; }
    public BusinessCase()
    {
        Header = new List<ExcelTableCell>();
        Exploration = new List<ExcelTableCell>();
        Capex = new List<ExcelTableCell>();
        OffshoreFacilites = new List<ExcelTableCell>();
        CessationOffshoreFacilites = new List<ExcelTableCell>();
        Drilling = new List<ExcelTableCell>();
        TotalAndAnnualOil = new List<ExcelTableCell>();
        NetSalesGas = new List<ExcelTableCell>();
        Co2Emissions = new List<ExcelTableCell>();
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

