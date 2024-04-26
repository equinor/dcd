using api.Models;
namespace api.Dtos;

public abstract class BaseUpdateTransportDto
{
    public string Name { get; set; } = string.Empty!;
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public Source Source { get; set; }
}
