using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Transports.Dtos.Update;

public abstract class BaseUpdateTransportDto
{
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
}
