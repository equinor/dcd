using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateNetSaleGasProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;

    public GenerateNetSaleGasProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
    }

    public async Task<NetSalesGasDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(drainageStrategy);
        var calculateNetSaleGas = CalculateNetSaleGas(drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = new NetSalesGas
        {
            StartYear = calculateNetSaleGas.StartYear,
            Values = calculateNetSaleGas.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(netSaleGas, project.PhysicalUnit);
        return dto ?? new NetSalesGasDto();
    }

    private static TimeSeries<double> CalculateNetSaleGas(DrainageStrategy drainageStrategy,
        TimeSeries<double> fuelConsumption, TimeSeries<double> flarings, TimeSeries<double> losses)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeries<double>();
        }

        var fuelFlaringLosses =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumption, flarings), losses);

        var negativeFuelFlaringLosses = new TimeSeriesVolume
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray(),
        };

        return TimeSeriesCost.MergeCostProfiles(drainageStrategy.ProductionProfileGas, negativeFuelFlaringLosses);
    }
}
