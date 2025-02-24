using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public static class ExplorationWellCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var profileTypes = new Dictionary<string, WellCategory>
        {
            { ProfileTypes.ExplorationWellCostProfile, WellCategory.Exploration_Well },
            { ProfileTypes.AppraisalWellCostProfile, WellCategory.Appraisal_Well },
            { ProfileTypes.SidetrackCostProfile, WellCategory.Sidetrack }
        };

        foreach (var profileType in profileTypes)
        {
            var wells = caseItem.Campaigns.SelectMany(x => x.ExplorationWells).Where(ew => ew.Well.WellCategory == profileType.Value).ToList();

            var profilesToMerge = new List<TimeSeries>();

            foreach (var explorationWell in wells)
            {
                if (explorationWell.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeries
                    {
                        Values = explorationWell.Values.Select(ds => ds * explorationWell.Well.WellCost).ToArray(),
                        StartYear = explorationWell.StartYear
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
