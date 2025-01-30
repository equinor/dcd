using api.Context;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public class ExplorationWellCostProfileService(DcdDbContext context)
{
    private static readonly IReadOnlyList<string> WellProfileTypes = new List<string>
    {
        ProfileTypes.ExplorationWellCostProfile,
        ProfileTypes.AppraisalWellCostProfile,
        ProfileTypes.SidetrackCostProfile
    };

    public async Task UpdateCostProfilesForWells(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.Exploration)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        await context.ExplorationWell
            .Include(x => x.DrillingSchedule)
            .Include(x => x.Well)
            .Where(x => caseItem.ExplorationLink == x.ExplorationId)
            .LoadAsync();

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => WellProfileTypes.Contains(x.ProfileType))
            .LoadAsync();

        RunCalculation(caseItem);
    }

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
            var wells = caseItem.Exploration!.ExplorationWells.Where(ew => ew.Well.WellCategory == profileType.Value).ToList();

            var profilesToMerge = new List<TimeSeriesCost>();

            foreach (var explorationWell in wells)
            {
                if (explorationWell.DrillingSchedule?.Values.Length > 0)
                {
                    profilesToMerge.Add(new TimeSeriesCost
                    {
                        Values = explorationWell.DrillingSchedule.Values.Select(ds => ds * explorationWell.Well.WellCost).ToArray(),
                        StartYear = explorationWell.DrillingSchedule.StartYear,
                    });
                }
            }

            var profileValues = TimeSeriesMerger.MergeTimeSeries(profilesToMerge);

            var sidetrackCostProfile = caseItem.CreateProfileIfNotExists(profileType.Key);

            sidetrackCostProfile.Values = profileValues.Values;
            sidetrackCostProfile.StartYear = profileValues.StartYear;
        }
    }
}
