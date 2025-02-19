using System.Globalization;

using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;

public static class GenerateGAndGAdminCostProfile
{
    public static void RunCalculation(Case caseItem, List<ExplorationWell> explorationWells)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true)
        {
            return;
        }

        if (explorationWells.Count == 0)
        {
            return;
        }

        var earliestYear = explorationWells.Select(ds => ds.StartYear).Min() + caseItem.DG4Date.Year;
        var dG1Date = caseItem.DG1Date;

        if (dG1Date.Year >= earliestYear)
        {
            var countryCost = MapCountry(caseItem.Project.Country);
            var lastYear = new DateTimeOffset(dG1Date.Year, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
            var lastYearMinutes = (dG1Date - lastYear).TotalMinutes;

            var totalMinutesLastYear = new TimeSpan(DateTime.IsLeapYear(lastYear.Year) ? 366 : 365, 0, 0, 0).TotalMinutes;
            var percentageOfLastYear = lastYearMinutes / totalMinutesLastYear;

            var gAndGAdminCost = caseItem.CreateProfileIfNotExists(ProfileTypes.GAndGAdminCost);

            gAndGAdminCost.StartYear = earliestYear - caseItem.DG4Date.Year;

            var years = lastYear.Year - earliestYear;
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
