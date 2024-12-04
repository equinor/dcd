using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupply.Dtos.Update;

public abstract class BaseUpdateOnshorePowerSupplyDto
{
    // public double GasExportPipelineLength { get; set; }
    // public double OilExportPipelineLength { get; set; }
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public Source Source { get; set; }
}
