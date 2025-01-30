using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Cases.Recalculation.Calculators.CalculateNpv;

public static class CalculateNpvService
{
    public static void RunCalculation(Case caseItem)
    {
        var cashflowProfile = GetCashflowProfile(caseItem);

        if (cashflowProfile == null)
        {
            caseItem.NPV = 0;
            return;
        }

        var discountRate = caseItem.Project.DiscountRate;

        if (discountRate == 0)
        {
            caseItem.NPV = 0;
            return;
        }

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;

        var npvValue = EconomicsHelper.CalculateDiscountedVolume(
                cashflowProfile.Values,
                discountRate,
                cashflowProfile.StartYear + Math.Abs(nextYearInRelationToDg4Year)
            );

        caseItem.NPV = npvValue / caseItem.Project.ExchangeRateUSDToNOK;
    }

    private static TimeSeriesCost? GetCashflowProfile(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile) == null ||
            caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile) == null)
        {
            return null;
        }

        var calculatedTotalIncomeCostProfile = new TimeSeriesCost(caseItem.GetProfile(ProfileTypes.CalculatedTotalIncomeCostProfile));
        var calculatedTotalCostCostProfile = new TimeSeriesCost(caseItem.GetProfile(ProfileTypes.CalculatedTotalCostCostProfile));

        return EconomicsHelper.CalculateCashFlow(calculatedTotalIncomeCostProfile, calculatedTotalCostCostProfile);
    }
}
