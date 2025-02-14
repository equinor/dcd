using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class CampaignDto
{
    [Required] public required Guid CampaignId { get; set; }
    [Required] public required CampaignType CampaignType { get; set; }
    [Required] public required double RigUpgradingCost { get; set; }
    [Required] public required double RigMobDemobCost { get; set; }
    [Required] public required RigProfileDto RigUpgradingProfile { get; set; }
    [Required] public required RigProfileDto RigMobDemobProfile { get; set; }
    [Required] public required List<CampaignWellDto> CampaignWells { get; set; }
}

public class RigProfileDto
{
    [Required] public required int StartYear { get; set; }
    [Required] public required double[] Values { get; set; }
}

public class CampaignWellDto
{
    [Required] public required Guid WellId { get; set; }
    [Required] public required string WellName { get; set; }
    [Required] public required WellCategory WellCategory { get; set; }
    [Required] public required int StartYear { get; set; }
    [Required] public required int[] Values { get; set; }
}
