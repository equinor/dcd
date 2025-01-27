using System.Globalization;

using api.Context;
using api.Features.Profiles;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;

public class GenerateGAndGAdminCostProfile(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.GAndGAdminCost || y.ProfileType == ProfileTypes.GAndGAdminCostOverride))
            .SingleAsync(x => x.Id == caseId);

        var exploration = await context.Explorations
            .SingleAsync(x => x.Id == caseItem.ExplorationLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true)
        {
            return;
        }

        var project = await context.Projects.SingleAsync(p => p.Id == caseItem.ProjectId);

        var linkedWells = await context.ExplorationWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.ExplorationId == exploration.Id).ToListAsync();

        if (linkedWells.Count > 0)
        {
            var drillingSchedules = linkedWells.Select(lw => lw.DrillingSchedule);
            var earliestYear = drillingSchedules.Select(ds => ds?.StartYear).Min() + caseItem.DG4Date.Year;
            var dG1Date = caseItem.DG1Date;
            if (earliestYear != null && dG1Date.Year >= earliestYear)
            {
                var countryCost = MapCountry(project.Country);
                var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
                var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

                var totalMinutesLastYear = new TimeSpan(DateTime.IsLeapYear(lastYear.Year) ? 366 : 365, 0, 0, 0).TotalMinutes;
                var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                var gAndGAdminCost = caseItem.CreateProfileIfNotExists(ProfileTypes.GAndGAdminCost);

                gAndGAdminCost.StartYear = (int)earliestYear - caseItem.DG4Date.Year;

                var years = lastYear.Year - (int)earliestYear;
                var values = new List<double>();
                for (int i = 0; i < years; i++)
                {
                    values.Add(countryCost);
                }
                values.Add(countryCost * percentageOfLastYear);

                gAndGAdminCost.Values = values.ToArray();
                gAndGAdminCost.StartYear = gAndGAdminCost.StartYear;
            }
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
