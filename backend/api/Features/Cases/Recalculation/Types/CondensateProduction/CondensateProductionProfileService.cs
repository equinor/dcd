
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.CondensateProduction;
public static class CondensateProductionProfileService
{
    public static void RunCalculation(Case caseItem)
    {

        var condensateYield = caseItem.DrainageStrategy.CondensateYield;
        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CondensateProduction);

        var gasProduction = GetGasProduction(caseItem);

        if (gasProduction.Values != null)
        {
            profile.StartYear = gasProduction.StartYear;
            profile.Values = gasProduction.Values
                    .Select(value => value * condensateYield / 1_000_000)
                    .ToArray();
        }
    }

    private static TimeSeries GetGasProduction(Case caseItem)
    {
        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        return TimeSeriesMerger.MergeTimeSeries(new TimeSeries(productionProfileGasProfile), new TimeSeries(additionalProductionProfileGasProfile));
    }
}
