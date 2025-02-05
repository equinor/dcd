using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.SaveCampaignWell;

public class SaveCampaignWellDto
{
    [Required] public required int StartYear { get; set; }
    [Required] public required int[] Values { get; set; }
}
