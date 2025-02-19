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

        var npvValue = EconomicsHelper.CalculateDiscountedVolume(
                cashflowProfile.Values,
                discountRate,
                cashflowProfile.StartYear,
                npvYearInRelationToDg4Year
            );

        caseItem.NPV = npvValue;
    }

    private static TimeSeries? GetCashflowProfile(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile) == null ||
            caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile) == null)
        {
            return null;
        }

        var calculatedTotalIncomeCostProfile = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalIncomeCostProfile));
        var calculatedTotalCostCostProfile = new TimeSeries(caseItem.GetProfile(ProfileTypes.CalculatedTotalCostCostProfile));

        return EconomicsHelper.CalculateCashFlow(calculatedTotalIncomeCostProfile, calculatedTotalCostCostProfile);
    }
}
