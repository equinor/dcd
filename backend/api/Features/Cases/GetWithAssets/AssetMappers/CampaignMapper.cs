using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class CampaignMapper
{
    public static CampaignDto MapToDto(Campaign campaign)
    {
        return new CampaignDto
        {
            CampaignId = campaign.Id,
            CampaignType = campaign.CampaignType,
            RigUpgradingCost = campaign.RigUpgradingCost,
            RigMobDemobCost = campaign.RigMobDemobCost,
            RigUpgradingProfile = new RigProfileDto
            {
                StartYear = campaign.RigUpgradingCostStartYear,
                Values = campaign.RigUpgradingCostValues
            },
            RigMobDemobProfile = new RigProfileDto
            {
                StartYear = campaign.RigMobDemobCostStartYear,
                Values = campaign.RigMobDemobCostValues
            },
            CampaignWells = campaign.CampaignWells
                .OrderBy(x => x.CreatedUtc)
                .Select(x => new CampaignWellDto
                {
                    WellId = x.WellId,
                    WellName = x.Well.Name ?? "",
                    WellCategory = x.Well.WellCategory,
                    StartYear = x.StartYear,
                    Values = x.Values
                })
                .ToList()
        };
    }
}
