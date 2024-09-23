using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class NetSaleGasProfileService : INetSaleGasProfileService
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;

    public NetSaleGasProfileService(
        ICaseService caseService,
        IProjectService projectService,
        ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService
        )
    {
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
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

        var topside = await _topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);
        var project = await _projectService.GetProject(caseItem.ProjectId);

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
            TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>?> { fuelConsumption, flarings, losses });

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
