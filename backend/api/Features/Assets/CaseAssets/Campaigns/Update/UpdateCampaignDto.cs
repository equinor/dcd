using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.CampaignWells.Save;

namespace api.Features.Assets.CaseAssets.Campaigns.Update;

public class UpdateCampaignDto
{
    [Required] public required double RigUpgradingCost { get; set; }
    [Required] public required int RigUpgradingCostStartYear { get; set; }
    [Required] public required double[] RigUpgradingCostValues { get; set; }

    [Required] public required double RigMobDemobCost { get; set; }
    [Required] public required int RigMobDemobCostStartYear { get; set; }
    [Required] public required double[] RigMobDemobCostValues { get; set; }

    [Required] public required List<SaveCampaignWellDto> CampaignWells { get; set; }
}
