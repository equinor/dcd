using api.Features.Profiles;
using api.Models;

namespace api.Features.Cases.Recalculation.Helpers;

public static class CalculationHelper
{
    public static int? GetRelativeLastYearOfProduction(Case caseItem)
    {
        var productionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        var lastYear = new List<int?>
        {
            productionProfileOilProfile?.Values.Length > 0 ? productionProfileOilProfile.StartYear + productionProfileOilProfile.Values.Length - 1 : null,
            additionalProductionProfileOilProfile?.Values.Length > 0 ? additionalProductionProfileOilProfile.StartYear + additionalProductionProfileOilProfile.Values.Length - 1 : null,
            productionProfileGasProfile?.Values.Length > 0 ? productionProfileGasProfile.StartYear + productionProfileGasProfile.Values.Length - 1 : null,
            additionalProductionProfileGasProfile?.Values.Length > 0 ? additionalProductionProfileGasProfile.StartYear + additionalProductionProfileGasProfile.Values.Length - 1 : null
        }.Where(year => year.HasValue).Max();

        return lastYear;
    }

    public static int? GetRelativeFirstYearOfProduction(Case caseItem)
    {
        var productionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil);
        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        var firstYear = new List<int?>
        {
            productionProfileOilProfile?.Values.Length > 0 ? productionProfileOilProfile.StartYear : null,
            additionalProductionProfileOilProfile?.Values.Length > 0 ? additionalProductionProfileOilProfile.StartYear : null,
            productionProfileGasProfile?.Values.Length > 0 ? productionProfileGasProfile.StartYear : null,
            additionalProductionProfileGasProfile?.Values.Length > 0 ? additionalProductionProfileGasProfile.StartYear : null
        }.Where(year => year.HasValue).Min();

        return firstYear;
    }

    public static void ResetTimeSeries(TimeSeriesProfile? timeSeries)
    {
        if (timeSeries == null)
        {
            return;
        }

        timeSeries.Values = [];
        timeSeries.StartYear = 0;
    }
}
