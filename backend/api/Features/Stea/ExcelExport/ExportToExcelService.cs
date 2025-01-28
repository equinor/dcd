using api.Features.Profiles.Dtos;
using api.Features.Stea.Dtos;

using ClosedXML.Excel;

namespace api.Features.Stea.ExcelExport;

public static class ExportToExcelService
{
    public static List<BusinessCase> CreateExcelCells(SteaProjectDto project)
    {
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Input to STEA");
        ws.Cell("B2").Value = project.Name;
        var rowCount = 3;
        var businessCases = new List<BusinessCase>();

        foreach (var c in project.SteaCases)
        {
            var businessCase = new BusinessCase();
            var headerRowCount = rowCount;
            rowCount++;
            businessCase.Exploration = CreateExcelRow("Exploration Cost", project.StartYear, c.Exploration, rowCount, 1);

            rowCount++;
            businessCase.Capex = CreateExcelRow("Capex", project.StartYear, c.Capex.Summary, rowCount, 1);

            rowCount++;
            businessCase.Drilling = CreateExcelRow("Drilling", project.StartYear, c.Capex.Drilling, rowCount, 1);

            rowCount++;
            businessCase.OffshoreFacilities = CreateExcelRow("Offshore Facilities", project.StartYear, c.Capex.OffshoreFacilities, rowCount, 1);

            rowCount++;
            businessCase.OnshorePowerSupply = CreateExcelRow("Onshore (Power from shore)", project.StartYear, c.Capex.OnshorePowerSupplyCost, rowCount, 1);

            rowCount++;
            businessCase.StudyCost = CreateExcelRow("Study cost", project.StartYear, c.StudyCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Opex = CreateExcelRow("Opex", project.StartYear, c.OpexCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Cessation = CreateExcelRow("Cessation - Offshore Facilities", project.StartYear, c.Capex.CessationCost, rowCount, 1);

            rowCount++;
            businessCase.ProductionAndSalesVolumes = new ExcelTableCell(ColumnNumber(1) + rowCount.ToString(), "Production And Sales Volumes");

            rowCount++;
            businessCase.TotalAndAnnualOil = CreateExcelRow("Total And annual Oil/Condensate production [MSm3]", project.StartYear, DivideTimeSeriesValuesByFactor(c.ProductionAndSalesVolumes.TotalAndAnnualOil, 1_000_000), rowCount, 1);

            rowCount++;
            businessCase.NetSalesGas = CreateExcelRow("Net Sales Gas [GSm3]", project.StartYear, DivideTimeSeriesValuesByFactor(c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas, 1_000_000_000), rowCount, 1);

            rowCount++;
            businessCase.Co2Emissions = CreateExcelRow("Co2 Emissions [mill tonnes]", project.StartYear, DivideTimeSeriesValuesByFactor(c.ProductionAndSalesVolumes.Co2Emissions, 1_000_000), rowCount, 1);

            rowCount++;
            businessCase.ImportedElectricity = CreateExcelRow("Imported electricity [GWh]", project.StartYear, c.ProductionAndSalesVolumes.ImportedElectricity, rowCount, 1);

            rowCount += 2;
            var allRows = new List<int>();
            if (c.Exploration.Values != null)
            {
                allRows.Add(c.Exploration.Values.Length + c.Exploration.StartYear);
            }
            if (c.Capex.Summary.Values != null)
            {
                allRows.Add(c.Capex.Summary.Values.Length + c.Capex.Summary.StartYear);
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
            if (c.ProductionAndSalesVolumes.AdditionalGas.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.AdditionalGas.Values.Length + c.ProductionAndSalesVolumes.AdditionalGas.StartYear);
            }
            if (c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values.Length + c.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
            }
            if (c.ProductionAndSalesVolumes.Co2Emissions.Values != null)
            {
                allRows.Add(c.ProductionAndSalesVolumes.Co2Emissions.Values.Length + c.ProductionAndSalesVolumes.Co2Emissions.StartYear);
            }

            var maxYear = allRows.Count > 0 ? allRows.Max() - project.StartYear : 0;

            businessCase.Header = CreateTableHeader(project.StartYear, project.StartYear + maxYear - 1, c.Name, headerRowCount);
            businessCases.Add(businessCase);
        }

        return businessCases;
    }

    private static void ValueToCells(List<ExcelTableCell> tableCells, int columnCount, int rowCount, TimeSeriesCostDto e, int projectStartYear, double factor)
    {
        if (e.Values == null)
        {
            return;
        }

        columnCount += e.StartYear - projectStartYear;
        columnCount++;

        for (var i = 0; i < e.Values.Length; i++)
        {
            var cellNo = ColumnNumber(columnCount++) + rowCount;
            tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Values[i]).ToString()));
        }
    }

    private static List<ExcelTableCell> CreateExcelRow(string title, int projectStartYear, TimeSeriesCostDto e, int rowCount, double factor)
    {
        var tableCells = new List<ExcelTableCell>();
        int columnCount = 1;
        string cellNo = ColumnNumber(columnCount++) + rowCount;
        tableCells.Add(new ExcelTableCell(cellNo, title));

        cellNo = ColumnNumber(columnCount) + rowCount;
        tableCells.Add(new ExcelTableCell(cellNo, (factor * e.Values.Sum()).ToString()));
        ValueToCells(tableCells, columnCount, rowCount, e, projectStartYear, factor);

        return tableCells;
    }

    private static List<ExcelTableCell> CreateTableHeader(int startYear, int endYear, string caseName, int rowCount)
    {
        var tableCells = new List<ExcelTableCell>();
        var columnCount = 1;
        var cellNo = ColumnNumber(columnCount++) + rowCount;
        tableCells.Add(new ExcelTableCell(cellNo, caseName));
        cellNo = ColumnNumber(columnCount++) + rowCount;
        tableCells.Add(new ExcelTableCell(cellNo, "Total Cost"));

        for (var i = startYear; i <= endYear; i++)
        {
            cellNo = ColumnNumber(columnCount++) + rowCount;
            tableCells.Add(new ExcelTableCell(cellNo, i.ToString()));
        }

        return tableCells;
    }

    private static string ColumnNumber(int cellNumber)
    {
        var rv = string.Empty;

        while (cellNumber > 0)
        {
            var rest = (cellNumber - 1) % 26;
            rv = ((char)(rest + 65)) + rv;
            cellNumber = (cellNumber - rest - 1) / 26;
        }

        return rv;
    }

    private static TimeSeriesCostDto DivideTimeSeriesValuesByFactor(TimeSeriesCostDto timeSeries, double factor)
    {
        return new TimeSeriesCostDto
        {
            StartYear = timeSeries.StartYear,
            Values = timeSeries.Values.Select(v => v / factor).ToArray()
        };
    }

    public static byte[] WriteExcelCellsToExcelDocument(List<BusinessCase> businessCases, string projectName)
    {
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Input to STEA");
        ws.Cell("B2").Value = projectName;

        foreach (var businessCase in businessCases)
        {
            foreach (var etc in businessCase.Header)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Exploration)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Capex)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Drilling)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.OffshoreFacilities)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.StudyCost)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Opex)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.OnshorePowerSupply)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Cessation)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            ws.Cell(businessCase.ProductionAndSalesVolumes.CellNo).Value = businessCase.ProductionAndSalesVolumes.Value;
            foreach (var etc in businessCase.TotalAndAnnualOil)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.NetSalesGas)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.Co2Emissions)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
            foreach (var etc in businessCase.ImportedElectricity)
            {
                ws.Cell(etc.CellNo).Value = etc.Value;
            }
        }

        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        return stream.ToArray();
    }
}
