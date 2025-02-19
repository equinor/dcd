using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.CampaignWells.Save;

public class SaveCampaignWellDto
{
    [Required] public required Guid WellId { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required int[] Values { get; set; }
}
