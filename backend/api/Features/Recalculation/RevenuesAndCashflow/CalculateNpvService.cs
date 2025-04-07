using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.RevenuesAndCashflow;

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

        caseItem.Npv = npvValue;
    }

    private static TimeSeries GetCashflowProfile(Case caseItem)
    {
        var calculatedTotalIncomeCostProfileUsd = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalIncomeCostProfile));
        var calculatedTotalCostCostProfileUsd = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalCostCostProfile));

        return EconomicsHelper.CalculateCashFlow(calculatedTotalIncomeCostProfileUsd, calculatedTotalCostCostProfileUsd);
    }
}
