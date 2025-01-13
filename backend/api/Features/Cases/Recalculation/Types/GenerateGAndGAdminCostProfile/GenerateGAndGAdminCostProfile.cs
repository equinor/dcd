using System.Globalization;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;

public class GenerateGAndGAdminCostProfile(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases.SingleAsync(x => x.Id == caseId);

        var exploration = await context.Explorations
            .Include(e => e.GAndGAdminCost)
            .Include(e => e.GAndGAdminCostOverride)
            .SingleAsync(x => x.Id == caseItem.ExplorationLink);

        if (exploration.GAndGAdminCostOverride?.Override == true)
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

                var totalMinutesLastYear = new TimeSpan(365, 0, 0, 0).TotalMinutes;
                var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

                var gAndGAdminCost = new GAndGAdminCost();

                gAndGAdminCost.StartYear = (int)earliestYear - caseItem.DG4Date.Year;

                var years = lastYear.Year - (int)earliestYear;
                var values = new List<double>();
                for (int i = 0; i < years; i++)
                {
                    values.Add(countryCost);
                }
                values.Add(countryCost * percentageOfLastYear);
                gAndGAdminCost.Values = values.ToArray();

                if (exploration.GAndGAdminCost != null)
                {
                    exploration.GAndGAdminCost.Values = gAndGAdminCost.Values;
                    exploration.GAndGAdminCost.StartYear = gAndGAdminCost.StartYear;
                }
                else
                {
                    exploration.GAndGAdminCost = gAndGAdminCost;
                }
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
