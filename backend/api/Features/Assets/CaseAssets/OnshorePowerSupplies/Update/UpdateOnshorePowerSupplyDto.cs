using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Update;

public class UpdateOnshorePowerSupplyDto
{
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
}
