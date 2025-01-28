using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.Helpers;

public static class CalculationHelper
{
    public static int? GetRelativeLastYearOfProduction(Case caseItem, DrainageStrategy drainageStrategy)
    {
        var productionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil);

        var lastYear = new List<int?>
        {
            productionProfileOilProfile?.Values.Length > 0 ? productionProfileOilProfile.StartYear + productionProfileOilProfile?.Values.Length - 1 : null,
            additionalProductionProfileOilProfile?.Values.Length > 0 ? additionalProductionProfileOilProfile.StartYear + additionalProductionProfileOilProfile?.Values.Length - 1 : null,
            drainageStrategy.ProductionProfileGas?.Values.Length > 0 ? drainageStrategy.ProductionProfileGas?.StartYear + drainageStrategy.ProductionProfileGas?.Values.Length - 1 : null,
            drainageStrategy.AdditionalProductionProfileGas?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileGas?.StartYear + drainageStrategy.AdditionalProductionProfileGas?.Values.Length - 1 : null
        }.Where(year => year.HasValue).Max();

        return lastYear;
    }

    public static int? GetRelativeFirstYearOfProduction(Case caseItem, DrainageStrategy drainageStrategy)
    {
        var productionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil);

        var firstYear = new List<int?>
        {
            productionProfileOilProfile?.Values.Length > 0 ? productionProfileOilProfile.StartYear : null,
            additionalProductionProfileOilProfile?.Values.Length > 0 ? additionalProductionProfileOilProfile.StartYear : null,
            drainageStrategy.ProductionProfileGas?.Values.Length > 0 ? drainageStrategy.ProductionProfileGas?.StartYear : null,
            drainageStrategy.AdditionalProductionProfileGas?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileGas?.StartYear : null
        }.Where(year => year.HasValue).Min();

        return firstYear;
    }

    public static void ResetTimeSeries(TimeSeriesProfile? timeSeries)
    {
        if (timeSeries != null)
        {
            timeSeries.Values = [];
            timeSeries.StartYear = 0;
        }
    }
}
