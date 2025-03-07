using api.Features.Cases.Recalculation.Calculators.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;

public static class CalculateTotalIncomeService
{
    public static void RunCalculation(Case caseItem)
    {
        var gasPriceNok = caseItem.Project.GasPriceNok;
        var oilPrice = caseItem.Project.OilPriceUsd;

        var totalOilProductionInMegaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil)
        );

        // Convert oil production from million sm³ to barrels in millions
        var oilProductionInMillionsOfBarrels = totalOilProductionInMegaCubics.Values.Select(v => v * BarrelsPerCubicMeter).ToArray();

        var oilIncome = new TimeSeries
        {
            StartYear = totalOilProductionInMegaCubics.StartYear,
            Values = oilProductionInMillionsOfBarrels.Select(v => v * oilPrice).ToArray()
        };

        var nglProduction = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl);
        var nglIncome = new TimeSeries();

        if (nglProduction != null && caseItem.Project.NglPriceUsd != 0)
        {
            nglIncome.StartYear = nglProduction.StartYear;
            nglIncome.Values = nglProduction.Values.Select(v => v * caseItem.Project.NglPriceUsd).ToArray();
        }

        var totalGasProductionInGigaCubics = EconomicsHelper.MergeProductionAndAdditionalProduction(
            caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas),
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas)
        );

        var gasIncome = new TimeSeries
        {
            StartYear = totalGasProductionInGigaCubics.StartYear,
            Values = totalGasProductionInGigaCubics.Values.Select(v => v * gasPriceNok / caseItem.Project.ExchangeRateUsdToNok).ToArray()
        };

        var totalIncome = TimeSeriesMerger.MergeTimeSeries(oilIncome, nglIncome, gasIncome);

        // Divide the totalIncome by 1 million before assigning it to CalculatedTotalIncomeCostProfileUsd to get correct unit
        var scaledTotalIncomeValues = totalIncome.Values.Select(v => v / 1_000_000).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CalculatedTotalIncomeCostProfileUsd);

        profile.Values = scaledTotalIncomeValues;
        profile.StartYear = totalIncome.StartYear;
    }
}
