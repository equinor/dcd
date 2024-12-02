namespace api.Features.Stea.ExcelExport;

public class ExcelTableCell(string cellNo, string value)
{
    public string CellNo { get; set; } = cellNo;
    public string Value { get; set; } = value;
}
