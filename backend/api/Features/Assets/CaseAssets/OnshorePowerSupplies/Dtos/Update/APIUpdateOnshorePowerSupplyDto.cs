using api.Models;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;

public class APIUpdateOnshorePowerSupplyDto : BaseUpdateOnshorePowerSupplyDto
{
    public Maturity Maturity { get; set; }
}