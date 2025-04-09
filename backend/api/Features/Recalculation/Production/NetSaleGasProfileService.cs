using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Features.Recalculation.Helpers;
using api.Models;
using api.Models.Enums;

namespace api.Features.Recalculation.Production;

// dependency order 2
public static class NetSaleGasProfileService
{
    /// <summary>
    /// (gas production - fuel flaring and losses)
    /// gas production is adjusted for gas shrinkage depending on the drainage strategy
    /// </summary>
    public static void RunCalculation(Case caseItem)
    {
        var totalGasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);
        var fuelFlaringAndLosses = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.FuelFlaringAndLosses));
        var drainageStrategy = caseItem.DrainageStrategy;

        var calculateNetSaleGas = EconomicsHelper.CalculateTotalGasSales(totalGasProduction, fuelFlaringAndLosses, drainageStrategy);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.NetSalesGas);

        profile.StartYear = calculateNetSaleGas.StartYear;
        profile.Values = calculateNetSaleGas.Values;
    }
}
