using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;

public abstract class BaseUpdateOnshorePowerSupplyDto
{
    public Currency Currency { get; set; }
    public int CostYear { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
    public Source Source { get; set; }
}
