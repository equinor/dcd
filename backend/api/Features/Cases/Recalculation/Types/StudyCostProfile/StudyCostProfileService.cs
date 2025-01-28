using System.Globalization;

using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.StudyCostProfile;

public class StudyCostProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .SingleAsync(x => x.Id == caseId);

        var sumFacilityCost = SumAllCostFacility(caseItem);
        var sumWellCost = SumWellCost(caseItem);

        CalculateTotalFeasibilityAndConceptStudies(caseItem, sumFacilityCost, sumWellCost);
        CalculateTotalFEEDStudies(caseItem, sumFacilityCost, sumWellCost);
    }

    public static void CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true)
        {
            return;
        }

        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

        var dg0 = caseItem.DG0Date;
        var dg2 = caseItem.DG2Date;

        if (dg0.Year == 1 || dg2.Year == 1)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies));
            return;
        }

        if (dg2.DayOfYear == 1) { dg2 = dg2.AddDays(-1); } // Treat the 1st of January as the 31st of December

        var totalDays = (dg2 - dg0).Days + 1;

        var firstYearDays = (new DateTimeOffset(dg0.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg0).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg2.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>
        {
            firstYearPercentage
        };
        for (int i = dg0.Year + 1; i < dg2.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalFeasibilityAndConceptStudies);

        profile.StartYear = dg0.Year - caseItem.DG4Date.Year;
        profile.Values = valuesList.ToArray();
    }

    public static void CalculateTotalFEEDStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)?.Override == true)
        {
            return;
        }

        var totalFeedStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFEEDStudies;

        var dg2 = caseItem.DG2Date;
        var dg3 = caseItem.DG3Date;

        if (dg2.Year == 1 || dg3.Year == 1)
        {
            CalculationHelper.ResetTimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies));
            return;
        }

        if (!DateIsEqual(dg2, dg3) && dg3.DayOfYear == 1) { dg3 = dg3.AddDays(-1); } // Treat the 1st of January as the 31st of December, only if dates are not equal

        var totalDays = (dg3 - dg2).Days + 1;

        if (totalDays == 0) // Might want to throw an exception here
        {
            totalDays = 1;
        }

        var firstYearDays = (new DateTimeOffset(dg2.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg2).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg3.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>
        {
            firstYearPercentage
        };
        for (int i = dg2.Year + 1; i < dg3.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeedStudies);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalFEEDStudies);

        profile.StartYear = dg2.Year - caseItem.DG4Date.Year;
        profile.Values = valuesList.ToArray();
    }

    private static double SumAllCostFacility(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile), caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile), caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile), caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile), caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride));
        sumFacilityCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile), caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride));

        return sumFacilityCost;
    }

    public static double SumWellCost(Case caseItem)
    {
        var sumWellCost = 0.0;

        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile), caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile), caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile), caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride));
        sumWellCost += SumOverrideOrProfile(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile), caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride));

        return sumWellCost;
    }

    private static bool DateIsEqual(DateTime date1, DateTime date2)
    {
        return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
    }

    private static double SumOverrideOrProfile(TimeSeriesProfile? profile, TimeSeriesProfile? profileOverride)
    {
        if (profileOverride?.Override == true)
        {
            return profileOverride.Values.Sum();
        }

        if (profile != null)
        {
            return profile.Values.Sum();
        }

        return 0;
    }
}
