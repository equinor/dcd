using System.ComponentModel.DataAnnotations;

namespace api.Features.Campaigns.Update.UpdateRigMobDemobCost;

public class UpdateRigMobDemobCostDto
{
    [Required] public required double Cost { get; set; }
}
