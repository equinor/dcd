using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

namespace api.Features.Recalculation.RevenuesAndCashflow;

// dependency order 4
public static class CalculateNpvService
{
    public static void RunCalculation(Case caseItem)
    {
        var cashflowProfile = GetCashflowProfile(caseItem);

        var discountRate = caseItem.Project.DiscountRate;

        if (discountRate == 0)
        {
            caseItem.Npv = 0;

            return;
        }

        var dg4Year = caseItem.Dg4Date.Year;
        var npvYearInRelationToDg4Year = caseItem.Project.NpvYear - dg4Year;

        var npvValue = EconomicsHelper.CalculateSumOfDiscountedVolume(
            cashflowProfile.Values,
            discountRate,
            cashflowProfile.StartYear,
            npvYearInRelationToDg4Year
        );

        var rate = caseItem.Project.Currency == Currency.Usd ? 1 : caseItem.Project.ExchangeRateUsdToNok;

        caseItem.Npv = npvValue / rate;
    }

    private static TimeSeries GetCashflowProfile(Case caseItem)
    {
        var calculatedTotalIncomeCostProfile = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalIncomeCostProfile));
        var calculatedTotalCostCostProfile = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalCostCostProfile));

        return EconomicsHelper.CalculateCashFlow(calculatedTotalIncomeCostProfile, calculatedTotalCostCostProfile);
    }
}
