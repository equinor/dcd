using System.ComponentModel.DataAnnotations;

namespace api.Features.Campaigns.Update.UpdateRigUpgradingCost;

public class UpdateRigUpgradingCostDto
{
    [Required] public required double Cost { get; set; }
}
