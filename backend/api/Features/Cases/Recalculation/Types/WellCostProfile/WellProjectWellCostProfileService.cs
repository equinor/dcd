using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public static class WellProjectWellCostProfileService
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
            var wells = caseItem.WellProject!.WellProjectWells.Where(wpw => wpw.Well.WellCategory == profileType.Value).ToList();

            var profilesToMerge = new List<TimeSeriesCost>();

            foreach (var wellProjectWell in wells)
            {
                if (wellProjectWell.DrillingSchedule?.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeriesCost
                    {
                        Values = wellProjectWell.DrillingSchedule.Values.Select(ds => ds * wellProjectWell.Well.WellCost).ToArray(),
                        StartYear = wellProjectWell.DrillingSchedule.StartYear,
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
