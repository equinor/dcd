using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class CampaignMapper
{
    public static CampaignDto MapToDto(Campaign campaign)
    {
        var wells = campaign.CampaignType == CampaignTypes.ExplorationCampaign
            ? MapExplorationWells(campaign.ExplorationWells)
            : MapDevelopmentWells(campaign.DevelopmentWells);

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
            CampaignWells = wells
        };
    }

    private static List<CampaignWellDto> MapExplorationWells(ICollection<ExplorationWell> explorationWells)
    {
        return explorationWells
            .Select(x => new CampaignWellDto
            {
                WellId = x.WellId,
                WellName = x.Well.Name ?? "",
                WellCategory = x.Well.WellCategory,
                StartYear = x.StartYear,
                Values = x.Values
            })
            .ToList();
    }

    private static List<CampaignWellDto> MapDevelopmentWells(ICollection<DevelopmentWell> developmentWells)
    {
        return developmentWells
            .Select(x => new CampaignWellDto
            {
                WellId = x.WellId,
                WellName = x.Well.Name ?? "",
                WellCategory = x.Well.WellCategory,
                StartYear = x.StartYear,
                Values = x.Values
            })
            .ToList();
    }
}
