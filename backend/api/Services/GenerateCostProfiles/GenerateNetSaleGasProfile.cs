using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateNetSaleGasProfile : IGenerateNetSaleGasProfile
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;

    public GenerateNetSaleGasProfile(ICaseService caseService, IProjectService projectService, ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService)
    {
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public NetSalesGasDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);
        var calculateNetSaleGas = CalculateNetSaleGas(drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = new NetSalesGas
        {
            StartYear = calculateNetSaleGas.StartYear,
            Values = calculateNetSaleGas.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert<NetSalesGasDto, NetSalesGas>(netSaleGas, project.PhysicalUnit);
        return dto ?? new NetSalesGasDto();
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
            TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { fuelConsumption, flarings, losses });

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

        return TimeSeriesCost.MergeCostProfiles(drainageStrategy.ProductionProfileGas, negativeFuelFlaringLosses);
    }
}
