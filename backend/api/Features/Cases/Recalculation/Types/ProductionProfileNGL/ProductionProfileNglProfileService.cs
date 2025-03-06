using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.ProductionProfileNGL;

public static class ProductionProfileNglProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl)?.Override == true)
        {
            return;
        }

        var nglYield = caseItem.DrainageStrategy.NGLYield;
        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.ProductionProfileNgl);

        var gasProduction = GetGasProduction(caseItem);

        profile.StartYear = gasProduction.StartYear;

        profile.Values = gasProduction.Values
            .Select(value => value * nglYield / 1_000_000)
            .ToArray();
    }

    private static TimeSeries GetGasProduction(Case caseItem)
    {
        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        return TimeSeriesMerger.MergeTimeSeries(new TimeSeries(productionProfileGasProfile), new TimeSeries(additionalProductionProfileGasProfile));
    }
}
