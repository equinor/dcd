using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;
using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.RevenuesAndCashflow;
// dependency order 4

public static class CalculateCashflowService
{
    /// <summary>
    /// Total income - total cost
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var totalCost = new TimeSeries( caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile));
        var totalIncomeUsd = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile));
        var totalCashflow = EconomicsHelper.CalculateCashFlow(totalIncomeUsd, totalCost);

        var calculatedCashflowProfile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalCashflow);

        calculatedCashflowProfile.StartYear = totalCashflow.StartYear;
        calculatedCashflowProfile.Values = totalCashflow.Values;
    }
}
