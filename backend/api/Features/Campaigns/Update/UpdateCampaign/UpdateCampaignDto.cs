using System.ComponentModel.DataAnnotations;

namespace api.Features.Campaigns.Update.UpdateCampaign;

public class UpdateCampaignDto
{
    [Required] public required CampaignCostType CampaignCostType { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
}

public enum CampaignCostType
{
    RigUpgrading,
    RigMobDemob
}
