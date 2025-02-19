using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaignCost;

public class UpdateCampaignCostDto
{
    [Required] public required double RigUpgradingCost { get; set; }
    [Required] public required double RigMobDemobCost { get; set; }
}
