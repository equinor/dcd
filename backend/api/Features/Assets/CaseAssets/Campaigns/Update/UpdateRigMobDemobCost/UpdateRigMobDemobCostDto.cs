using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.Update.UpdateRigMobDemobCost;

public class UpdateRigMobDemobCostDto
{
    [Required] public required double Cost { get; set; }
} 