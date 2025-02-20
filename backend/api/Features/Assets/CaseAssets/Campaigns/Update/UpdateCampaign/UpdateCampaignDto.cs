using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateCampaign;

public class UpdateCampaignDto
{
    [Required] public required int RigUpgradingCostStartYear { get; set; }
    [Required] public required double[] RigUpgradingCostValues { get; set; }

    [Required] public required int RigMobDemobCostStartYear { get; set; }
    [Required] public required double[] RigMobDemobCostValues { get; set; }
}
