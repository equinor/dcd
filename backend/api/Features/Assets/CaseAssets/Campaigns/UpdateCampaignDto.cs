namespace api.Features.Assets.CaseAssets.Campaigns;

public class UpdateCampaignDto
{
    public required double RigUpgradingCost { get; set; }
    public required int RigUpgradingCostStartYear { get; set; }
    public required double[] RigUpgradingCostValues { get; set; }

    public required double RigMobDemobCost { get; set; }
    public required int RigMobDemobCostStartYear { get; set; }
    public required double[] RigMobDemobCostValues { get; set; }
}
