using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class NetSaleGasProfileService(
    ICaseService caseService,
    ITopsideService topsideService,
    IDrainageStrategyService drainageStrategyService)
    : INetSaleGasProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.NetSalesGas!,
            d => d.NetSalesGasOverride!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileWaterInjection!
        );

        if (drainageStrategy.NetSalesGasOverride?.Override == true)
        {
            return;
        }

        var topside = await topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);
        var project = await caseService.GetProject(caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var calculateNetSaleGas = CalculateNetSaleGas(drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = new NetSalesGas
        {
            StartYear = calculateNetSaleGas.StartYear,
            Values = calculateNetSaleGas.Values
        };

        if (drainageStrategy.NetSalesGas != null)
        {
            drainageStrategy.NetSalesGas.StartYear = netSaleGas.StartYear;
            drainageStrategy.NetSalesGas.Values = netSaleGas.Values;
        }
        else
        {
            drainageStrategy.NetSalesGas = new NetSalesGas
            {
                StartYear = netSaleGas.StartYear,
                Values = netSaleGas.Values
            };
        }

    }

    private static TimeSeries<double> CalculateNetSaleGas(DrainageStrategy drainageStrategy,
        TimeSeries<double> fuelConsumption, TimeSeries<double> flarings, TimeSeries<double> losses)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeries<double>();
        }

        if (drainageStrategy.GasSolution == GasSolution.Injection)
        {
            return new TimeSeries<double>();
        }

        var fuelFlaringLosses =
            TimeSeriesCost.MergeCostProfilesList([fuelConsumption, flarings, losses]);

        if (drainageStrategy.FuelFlaringAndLossesOverride?.Override == true)
        {
            fuelFlaringLosses.StartYear = drainageStrategy.FuelFlaringAndLossesOverride.StartYear;
            fuelFlaringLosses.Values = drainageStrategy.FuelFlaringAndLossesOverride.Values;
        }

        var negativeFuelFlaringLosses = new TimeSeriesVolume
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray(),
        };

        var additionalProductionProfileGas = drainageStrategy.AdditionalProductionProfileGas ?? new TimeSeries<double>();

        var gasProduction = TimeSeriesCost.MergeCostProfiles(drainageStrategy.ProductionProfileGas, additionalProductionProfileGas);
        return TimeSeriesCost.MergeCostProfiles(gasProduction, negativeFuelFlaringLosses);
    }

}
