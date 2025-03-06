using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public static class DevelopmentWellCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var profileTypes = new Dictionary<string, WellCategory>
        {
            { ProfileTypes.OilProducerCostProfile, WellCategory.OilProducer },
            { ProfileTypes.GasProducerCostProfile, WellCategory.GasProducer },
            { ProfileTypes.WaterInjectorCostProfile, WellCategory.WaterInjector },
            { ProfileTypes.GasInjectorCostProfile, WellCategory.GasInjector }
        };

        foreach (var profileType in profileTypes)
        {
            List<CampaignWell> wells = [];

            foreach (var campaign in caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.DevelopmentCampaign))
            {
                foreach (var developmentWell in campaign.CampaignWells)
                {
                    if (developmentWell.Well.WellCategory == profileType.Value)
                    {
                        wells.Add(developmentWell);
                    }
                }
            }

            var profilesToMerge = new List<TimeSeries>();

            foreach (var developmentWell in wells)
            {
                if (developmentWell.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeries
                    {
                        Values = developmentWell.Values.Select(ds => ds * developmentWell.Well.WellCost).ToArray(),
                        StartYear = developmentWell.StartYear
                    });
                }
            }

            var profileValues = TimeSeriesMerger.MergeTimeSeries(profilesToMerge);

            var profile = caseItem.CreateProfileIfNotExists(profileType.Key);

            profile.Values = profileValues.Values;
            profile.StartYear = profileValues.StartYear;
        }
    }
}
