using System.Globalization;

using api.Context;
using api.Features.Profiles;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;

public class GenerateGAndGAdminCostProfile(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.GAndGAdminCost,
            ProfileTypes.GAndGAdminCostOverride
        };

        var caseItem = await context.Cases
            .Include(x => x.Project)
            .SingleAsync(x => x.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => profileTypes.Contains(x.ProfileType))
            .LoadAsync();

        var drillingSchedulesForExplorationWell = await context.ExplorationWell
            .Where(w => w.ExplorationId == caseItem.ExplorationLink)
            .Select(x => x.DrillingSchedule)
            .Where(x => x != null)
            .Select(x => x!)
            .ToListAsync();

        RunCalculation(caseItem, drillingSchedulesForExplorationWell);
    }

    public static void RunCalculation(Case caseItem, List<DrillingSchedule> drillingSchedulesForExplorationWell)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true)
        {
            return;
        }

        if (drillingSchedulesForExplorationWell.Count == 0)
        {
            return;
        }

        var earliestYear = drillingSchedulesForExplorationWell.Select(ds => ds.StartYear).Min() + caseItem.DG4Date.Year;
        var dG1Date = caseItem.DG1Date;

        if (dG1Date.Year >= earliestYear)
        {
            var countryCost = MapCountry(caseItem.Project.Country);
            var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
            var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

            var totalMinutesLastYear = new TimeSpan(DateTime.IsLeapYear(lastYear.Year) ? 366 : 365, 0, 0, 0).TotalMinutes;
            var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

            var gAndGAdminCost = caseItem.CreateProfileIfNotExists(ProfileTypes.GAndGAdminCost);

            gAndGAdminCost.StartYear = (int)earliestYear - caseItem.DG4Date.Year;

            var years = lastYear.Year - (int)earliestYear;
            var values = new List<double>();

            for (var i = 0; i < years; i++)
            {
                values.Add(countryCost);
            }

            values.Add(countryCost * percentageOfLastYear);

            gAndGAdminCost.Values = values.ToArray();
            gAndGAdminCost.StartYear = gAndGAdminCost.StartYear;
        }
    }

    private static double MapCountry(string country)
    {
        return country switch
        {
            "NORWAY" => 1,
            "UK" => 1,
            "BRAZIL" => 3,
            "CANADA" => 3,
            "UNITED STATES" => 3,
            _ => 7.0,
        };
    }
}
