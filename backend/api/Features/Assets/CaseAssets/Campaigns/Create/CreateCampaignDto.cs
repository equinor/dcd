using System.ComponentModel.DataAnnotations;

namespace api.Features.Assets.CaseAssets.Campaigns.Create;

public class CreateCampaignDto
{
    [Required] public required string CampaignType { get; set; }
}
