using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
// dependency order 1
namespace api.Features.Recalculation.Production;

public static class CondensateProductionProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var condensateYield = caseItem.DrainageStrategy.CondensateYield;
        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.CondensateProduction);

        var gasProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileGas);

        profile.StartYear = gasProduction.StartYear;

        profile.Values = gasProduction.Values
            .Select(value => value * condensateYield / 1_000_000)
            .ToArray();
    }
}
