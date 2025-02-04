using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public static class DevelopmentWellCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var profileTypes = new Dictionary<string, WellCategory>
        {
            { ProfileTypes.OilProducerCostProfile, WellCategory.Oil_Producer },
            { ProfileTypes.GasProducerCostProfile, WellCategory.Gas_Producer },
            { ProfileTypes.WaterInjectorCostProfile, WellCategory.Water_Injector },
            { ProfileTypes.GasInjectorCostProfile, WellCategory.Gas_Injector }
        };

        foreach (var profileType in profileTypes)
        {
            var wells = caseItem.WellProject!.DevelopmentWells.Where(wpw => wpw.Well.WellCategory == profileType.Value).ToList();

            var profilesToMerge = new List<TimeSeriesCost>();

            foreach (var developmentWell in wells)
            {
                if (developmentWell.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeriesCost
                    {
                        Values = developmentWell.Values.Select(ds => ds * developmentWell.Well.WellCost).ToArray(),
                        StartYear = developmentWell.StartYear,
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
