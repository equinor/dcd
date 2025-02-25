using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateRigUpgradingCost;

public class UpdateRigUpgradingCostDto
{
    [Required] public required double Cost { get; set; }
}
