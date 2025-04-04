using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

namespace api.Features.Recalculation.Production;

public static class NetSaleGasProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            return;
        }

        var calculateNetSaleGas = CalculateNetSaleGas(caseItem);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.NetSalesGas);

        profile.StartYear = calculateNetSaleGas.StartYear;
        profile.Values = calculateNetSaleGas.Values;
    }

    private static TimeSeries CalculateNetSaleGas(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas) == null)
        {
            return new TimeSeries();
        }

        if (caseItem.DrainageStrategy.GasSolution == GasSolution.Injection)
        {
            return new TimeSeries();
        }

        var fuelFlaringLosses = EmissionCalculationHelper.CalculateFuelFlaringAndLosses(caseItem);

        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            fuelFlaringLosses.StartYear = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).StartYear;
            fuelFlaringLosses.Values = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).Values;
        }

        var negativeFuelFlaringLosses = new TimeSeries
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray()
        };

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        var additionalProductionProfileGas = new TimeSeries(additionalProductionProfileGasProfile);

        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var productionProfileGasTimeSeries = new TimeSeries(productionProfileGasProfile);

        var gasProduction = TimeSeriesMerger.MergeTimeSeries(productionProfileGasTimeSeries, additionalProductionProfileGas);
        var nglYield = caseItem.DrainageStrategy.NglYield;
        var condensateYield = caseItem.DrainageStrategy.CondensateYield;
        var gasShrinkageFactor = caseItem.DrainageStrategy.GasShrinkageFactor;

        if (nglYield + condensateYield <= 0)
        {
            return TimeSeriesMerger.MergeTimeSeries(gasProduction, negativeFuelFlaringLosses);
        }

        var gasAdjustedForShrinkageFactor = new TimeSeries
        {
            StartYear = gasProduction.StartYear,
            Values = gasProduction.Values.Select(value => value * (gasShrinkageFactor / 100)).ToArray()
        };

        return TimeSeriesMerger.MergeTimeSeries(gasAdjustedForShrinkageFactor, negativeFuelFlaringLosses);
    }
}
