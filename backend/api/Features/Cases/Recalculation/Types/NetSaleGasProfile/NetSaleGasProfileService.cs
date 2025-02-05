using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.NetSaleGasProfile;

public static class NetSaleGasProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            return;
        }

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem);
        var flarings = EmissionCalculationHelper.CalculateFlaring(caseItem);
        var losses = EmissionCalculationHelper.CalculateLosses(caseItem);

        var calculateNetSaleGas = CalculateNetSaleGas(caseItem, fuelConsumptions, flarings, losses);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.NetSalesGas);

        profile.StartYear = calculateNetSaleGas.StartYear;
        profile.Values = calculateNetSaleGas.Values;
    }

    private static TimeSeriesCost CalculateNetSaleGas(Case caseItem,
        TimeSeriesCost fuelConsumption,
        TimeSeriesCost flarings,
        TimeSeriesCost losses)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas) == null)
        {
            return new TimeSeriesCost();
        }

        if (caseItem.DrainageStrategy!.GasSolution == GasSolution.Injection)
        {
            return new TimeSeriesCost();
        }

        var fuelFlaringLosses = TimeSeriesMerger.MergeTimeSeries(fuelConsumption, flarings, losses);

        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            fuelFlaringLosses.StartYear = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).StartYear;
            fuelFlaringLosses.Values = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).Values;
        }

        var negativeFuelFlaringLosses = new TimeSeriesCost
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray()
        };

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        var additionalProductionProfileGas = new TimeSeriesCost(additionalProductionProfileGasProfile);

        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var productionProfileGasTimeSeries = new TimeSeriesCost(productionProfileGasProfile);

        var gasProduction = TimeSeriesMerger.MergeTimeSeries(productionProfileGasTimeSeries, additionalProductionProfileGas);
        return TimeSeriesMerger.MergeTimeSeries(gasProduction, negativeFuelFlaringLosses);
    }
}
