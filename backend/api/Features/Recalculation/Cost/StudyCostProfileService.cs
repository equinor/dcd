using api.Features.Profiles;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.Cost;

public static class StudyCostProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var sumFacilityCost = SumAllCostFacility(caseItem);
        var sumWellCost = SumWellCost(caseItem);

        CalculateTotalFeasibilityAndConceptStudies(caseItem, sumFacilityCost, sumWellCost);
        CalculateTotalFeedStudies(caseItem, sumFacilityCost, sumWellCost);
    }

    private static void CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true)
        {
            return;
        }

        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

        if (caseItem.Dg0Date == null || caseItem.Dg2Date == null)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies));

            return;
        }

        var dg0 = caseItem.Dg0Date.Value;
        var dg2 = caseItem.Dg2Date.Value;

        if (dg2.DayOfYear == 1)
        {
            // Treat the 1st of January as the 31st of December
            dg2 = dg2.AddDays(-1);
        }

        var totalDays = (dg2 - dg0).Days + 1;

        var firstYearDays = DateTime.IsLeapYear(dg0.Year) ? 366 : 365;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg2.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>
        {
            firstYearPercentage
        };

        for (var i = dg0.Year + 1; i < dg2.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }

        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalFeasibilityAndConceptStudies);

        profile.StartYear = dg0.Year - caseItem.Dg4Date.Year;
        profile.Values = valuesList.ToArray();
    }

    public static void CalculateTotalFeedStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudiesOverride)?.Override == true)
        {
            return;
        }

        if (caseItem.Dg2Date == null || caseItem.Dg3Date == null || caseItem.Dg3Date.Value < caseItem.Dg2Date.Value)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudies));

            return;
        }

        var dg2 = caseItem.Dg2Date.Value;
        var dg3 = caseItem.Dg3Date.Value;

        if (!DateIsEqual(dg2, dg3) && dg3.DayOfYear == 1)
        {
            // Treat the 1st of January as the 31st of December, only if dates are not equal
            dg3 = dg3.AddDays(-1);
        }

        var totalDays = (dg3 - dg2).Days + 1;

        var totalFeedStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeedStudies;

        var costPerDay = totalFeedStudies / totalDays;

        var costsPerYear = new List<double>();

        for (var year = dg2.Year; year <= dg3.Year; year++)
        {
            var startDateForYear = dg2.Year == year ? dg2 : new DateTime(year, 1, 1);
            var endDateForYear = dg3.Year == year ? dg3 : new DateTime(year, 12, 31);

            var daysInYear = (endDateForYear - startDateForYear).TotalDays + 1;
            costsPerYear.Add(daysInYear * costPerDay);
        }

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalFeedStudies);

        profile.StartYear = dg2.Year - caseItem.Dg4Date.Year;
        profile.Values = costsPerYear.ToArray();
    }

    private static double SumAllCostFacility(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.SubstructureCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.SurfCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.TopsideCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.TransportCostProfile)?.Values.Sum() ?? 0;
        sumFacilityCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.OnshorePowerSupplyCostProfile)?.Values.Sum() ?? 0;

        return sumFacilityCost;
    }

    private static double SumWellCost(Case caseItem)
    {
        var sumWellCost = 0.0;

        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.OilProducerCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasProducerCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.WaterInjectorCostProfile)?.Values.Sum() ?? 0;
        sumWellCost += caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasInjectorCostProfile)?.Values.Sum() ?? 0;

        return sumWellCost;
    }

    private static bool DateIsEqual(DateTime date1, DateTime date2)
    {
        return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
    }
}
