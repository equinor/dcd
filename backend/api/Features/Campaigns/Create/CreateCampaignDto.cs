using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Campaigns.Create;

public class CreateCampaignDto
{
    [Required] public required CampaignType CampaignType { get; set; }
}
