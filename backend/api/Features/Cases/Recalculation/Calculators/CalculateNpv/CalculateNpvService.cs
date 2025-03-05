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

        var dg4Year = caseItem.DG4Date.Year;
        var npvYearInRelationToDg4Year = caseItem.Project.NpvYear - dg4Year;

        var npvValue = EconomicsHelper.CalculateSumOfDiscountedVolume(
            cashflowProfile.Values,
            discountRate,
            cashflowProfile.StartYear,
            npvYearInRelationToDg4Year
        );

        caseItem.NPV = npvValue;
    }

    private static TimeSeries? GetCashflowProfile(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfileUsd) == null ||
            caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfileUsd) == null)
        {
            return null;
        }

        var calculatedTotalIncomeCostProfileUsd = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalIncomeCostProfileUsd));
        var calculatedTotalCostCostProfileUsd = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalCostCostProfileUsd));

        return EconomicsHelper.CalculateCashFlow(calculatedTotalIncomeCostProfileUsd, calculatedTotalCostCostProfileUsd);
    }
}
