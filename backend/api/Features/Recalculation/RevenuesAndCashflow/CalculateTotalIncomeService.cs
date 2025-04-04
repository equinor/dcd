using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

using static api.Features.Profiles.CalculationConstants;

namespace api.Features.Recalculation.RevenuesAndCashflow;

public static class CalculateTotalIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var oilPrice = caseItem.Project.OilPriceUsd;

        var totalOilProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);

        var oilProductionInBarrels = totalOilProduction.Values.Select(v => v * BarrelsPerCubicMeter).ToArray();

        var oilIncome = new TimeSeries
        {
            StartYear = totalOilProduction.StartYear,
            Values = oilProductionInBarrels.Select(v => v * oilPrice).ToArray()
        };

        var nglProduction = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl);
        var nglIncome = new TimeSeries();

        if (nglProduction != null && caseItem.Project.NglPriceUsd != 0)
        {
            nglIncome.StartYear = nglProduction.StartYear;
            nglIncome.Values = nglProduction.Values.Select(v => v * caseItem.Project.NglPriceUsd).ToArray();
        }

        var totalGasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);

        var gasIncome = new TimeSeries
        {
            StartYear = totalGasProduction.StartYear,
            Values = totalGasProduction.Values.Select(v => v * gasPriceNok / caseItem.Project.ExchangeRateUsdToNok).ToArray()
        };

        var totalIncome = TimeSeriesMerger.MergeTimeSeries(oilIncome, nglIncome, gasIncome);

        var scaledTotalIncomeValues = totalIncome.Values.Select(v => v / 1_000_000).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalIncomeCostProfileUsd);

        profile.Values = scaledTotalIncomeValues;
        profile.StartYear = totalIncome.StartYear;
    }
}
