using api.Features.Profiles.Dtos;
using api.Features.Stea.Dtos;

using ClosedXML.Excel;

namespace api.Features.Stea.ExcelExport;

public static class ExportToExcelService
{
    public static byte[] ExportToExcel(string projectName, List<SteaCaseDto> steaCaseDtos)
    {
        var startYears = steaCaseDtos.Where(x => x.StartYear > 0).Select(x => x.StartYear).ToList();
        var startYear = startYears.Any() ? startYears.Min() : 0;

        var businessCases = CreateExcelCells(projectName, steaCaseDtos, startYear);

        return WriteExcelCellsToExcelDocument(businessCases, projectName);
    }

    private static List<BusinessCase> CreateExcelCells(string projectName, List<SteaCaseDto> steaCaseDtos, int startYear)
    {
        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Input to STEA");
        ws.Cell("B2").Value = projectName;
        var rowCount = 3;
        var businessCases = new List<BusinessCase>();

        foreach (var steaCaseDto in steaCaseDtos)
        {
            var businessCase = new BusinessCase();
            var headerRowCount = rowCount;
            rowCount++;
            businessCase.Exploration = CreateExcelRow("Exploration Cost", startYear, steaCaseDto.Exploration, rowCount, 1);

            rowCount++;
            businessCase.Capex = CreateExcelRow("Capex", startYear, steaCaseDto.Capex.Summary, rowCount, 1);

            rowCount++;
            businessCase.Drilling = CreateExcelRow("Drilling", startYear, steaCaseDto.Capex.Drilling, rowCount, 1);

            rowCount++;
            businessCase.OffshoreFacilities = CreateExcelRow("Offshore Facilities", startYear, steaCaseDto.Capex.OffshoreFacilities, rowCount, 1);

            rowCount++;
            businessCase.OnshorePowerSupply = CreateExcelRow("Onshore (Power from shore)", startYear, steaCaseDto.Capex.OnshorePowerSupplyCost, rowCount, 1);

            rowCount++;
            businessCase.StudyCost = CreateExcelRow("Study cost", startYear, steaCaseDto.StudyCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Opex = CreateExcelRow("Opex", startYear, steaCaseDto.OpexCostProfile, rowCount, 1);

            rowCount++;
            businessCase.Cessation = CreateExcelRow("Cessation - Offshore Facilities", startYear, steaCaseDto.Capex.CessationCost, rowCount, 1);

            rowCount++;
            businessCase.ProductionAndSalesVolumes = new ExcelTableCell(ColumnNumber(1) + rowCount, "Production And Sales Volumes");

            rowCount++;
            businessCase.TotalAndAnnualOil = CreateExcelRow("Total And annual Oil/Condensate production [MSm3]", startYear, DivideTimeSeriesValuesByFactor(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil, 1_000_000), rowCount, 1);

            rowCount++;
            businessCase.NetSalesGas = CreateExcelRow("Net Sales Gas [GSm3]", startYear, DivideTimeSeriesValuesByFactor(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas, 1_000_000_000), rowCount, 1);

            rowCount++;
            businessCase.Co2Emissions = CreateExcelRow("Co2 Emissions [mill tonnes]", startYear, DivideTimeSeriesValuesByFactor(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions, 1_000_000), rowCount, 1);

            rowCount++;
            businessCase.ImportedElectricity = CreateExcelRow("Imported electricity [GWh]", startYear, steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity, rowCount, 1);

            rowCount += 2;

            var allRows = new List<int>
            {
                steaCaseDto.Exploration.Values.Length + steaCaseDto.Exploration.StartYear,
                steaCaseDto.Capex.Summary.Values.Length + steaCaseDto.Capex.Summary.StartYear,
                steaCaseDto.Capex.CessationCost.Values.Length + steaCaseDto.Capex.CessationCost.StartYear,
                steaCaseDto.OpexCostProfile.Values.Length + steaCaseDto.OpexCostProfile.StartYear,
                steaCaseDto.StudyCostProfile.Values.Length + steaCaseDto.StudyCostProfile.StartYear,
                steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.Values.Length + steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear,
                steaCaseDto.ProductionAndSalesVolumes.AdditionalGas.Values.Length + steaCaseDto.ProductionAndSalesVolumes.AdditionalGas.StartYear,
                steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.Values.Length + steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear,
                steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.Values.Length + steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear
            };

            var maxYear = allRows.Count > 0 ? allRows.Max() - startYear : 0;

            businessCase.Header = CreateTableHeader(startYear, startYear + maxYear - 1, steaCaseDto.Name, headerRowCount);
            businessCases.Add(businessCase);
        }

        return businessCases;
    }

    private static void ValueToCells(List<ExcelTableCell> tableCells, int columnCount, int rowCount, TimeSeries e, int projectStartYear, double factor)
    {
        columnCount += e.StartYear - projectStartYear;
        columnCount++;

        foreach (var value in e.Values)
        {
            var cellNo = ColumnNumber(columnCount++) + rowCount;
            tableCells.Add(new ExcelTableCell(cellNo, (factor * value).ToString()));
        }
    }

    private static List<ExcelTableCell> CreateExcelRow(string title, int projectStartYear, TimeSeries e, int rowCount, double factor)
    {
        var tableCells = new List<ExcelTableCell>();
        var columnCount = 1;
        var cellNo = ColumnNumber(columnCount++) + rowCount;
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

    private static TimeSeries DivideTimeSeriesValuesByFactor(TimeSeries timeSeries, double factor)
    {
        return new TimeSeries
        {
            StartYear = timeSeries.StartYear,
            Values = timeSeries.Values.Select(v => v / factor).ToArray()
        };
    }

    private static byte[] WriteExcelCellsToExcelDocument(List<BusinessCase> businessCases, string projectName)
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
