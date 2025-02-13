using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Assets.CaseAssets.Campaigns.Create;

public class CreateCampaignDto
{
    [Required] public required CampaignType CampaignType { get; set; }
}
