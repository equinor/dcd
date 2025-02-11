using api.Models;
using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies;

public class UpdateOnshorePowerSupplyDto
{
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
}

public class ProspUpdateOnshorePowerSupplyDto
{
    public int CostYear { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
}
