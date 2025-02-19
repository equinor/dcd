using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.CampaignWells.Save;

namespace api.Features.Assets.CaseAssets.Campaigns.Update;

public class UpdateCampaignWellsDto
{
    [Required] public required List<SaveCampaignWellDto> CampaignWells { get; set; }
}
