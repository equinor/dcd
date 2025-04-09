using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.RevenuesAndCashflow;
// dependency order 5

public static class CalculatedDiscountedCashflowService
{
    public static void RunCalculation(Case caseItem)
    {
        var discountRate = caseItem.Project.DiscountRate;
        var npvYear = caseItem.Project.NpvYear;
        var dg4Year = caseItem.Dg4Date.Year;

        var totalCashflow = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCashflow));

        var discountedCashFlow = EconomicsHelper.CalculateWithDiscountFactor(discountRate, npvYear, dg4Year, totalCashflow);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedDiscountedCashflow);
        profile.Values = discountedCashFlow.Values;
        profile.StartYear = discountedCashFlow.StartYear;
    }

}
