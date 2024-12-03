using api.Models;

namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public static class CalculationHelper
{
    public static int? GetRelativeLastYearOfProduction(DrainageStrategy drainageStrategy)
    {
        var lastYear = new List<int?>
        {
            drainageStrategy.ProductionProfileOil?.Values.Length > 0 ? drainageStrategy.ProductionProfileOil?.StartYear + drainageStrategy.ProductionProfileOil?.Values.Length - 1 : null,
            drainageStrategy.AdditionalProductionProfileOil?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileOil?.StartYear + drainageStrategy.AdditionalProductionProfileOil?.Values.Length - 1 : null,
            drainageStrategy.ProductionProfileGas?.Values.Length > 0 ? drainageStrategy.ProductionProfileGas?.StartYear + drainageStrategy.ProductionProfileGas?.Values.Length - 1 : null,
            drainageStrategy.AdditionalProductionProfileGas?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileGas?.StartYear + drainageStrategy.AdditionalProductionProfileGas?.Values.Length - 1 : null
        }.Where(year => year.HasValue).Max();

        return lastYear;
    }

    public static int? GetRelativeFirstYearOfProduction(DrainageStrategy drainageStrategy)
    {
        var firstYear = new List<int?>
        {
            drainageStrategy.ProductionProfileOil?.Values.Length > 0 ? drainageStrategy.ProductionProfileOil?.StartYear : null,
            drainageStrategy.AdditionalProductionProfileOil?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileOil?.StartYear : null,
            drainageStrategy.ProductionProfileGas?.Values.Length > 0 ? drainageStrategy.ProductionProfileGas?.StartYear : null,
            drainageStrategy.AdditionalProductionProfileGas?.Values.Length > 0 ? drainageStrategy.AdditionalProductionProfileGas?.StartYear : null
        }.Where(year => year.HasValue).Min();

        return firstYear;
    }

    public static void ResetTimeSeries(TimeSeries<double>? timeSeries)
    {
        if (timeSeries != null)
        {
            timeSeries.Values = [];
            timeSeries.StartYear = 0;
        }
    }
}
