using api.Adapters;
using api.Context;
using api.Dtos;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class NetSaleGasProfileService : INetSaleGasProfileService
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public NetSaleGasProfileService(
        DcdDbContext context,
        ICaseService caseService,
        IProjectService projectService,
        ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService,
        IMapper mapper
        )
    {
        _context = context;
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
        _mapper = mapper;
    }

    public async Task<NetSalesGasDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);
        var calculateNetSaleGas = CalculateNetSaleGas(drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = drainageStrategy.NetSalesGas ?? new NetSalesGas();

        netSaleGas.StartYear = calculateNetSaleGas.StartYear;
        netSaleGas.Values = calculateNetSaleGas.Values;

        UpdateDrainageStrategyAndSave(drainageStrategy, netSaleGas);

        var dto = _mapper.Map<NetSalesGasDto>(netSaleGas, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        return dto ?? new NetSalesGasDto();
    }

    private void UpdateDrainageStrategyAndSave(DrainageStrategy drainageStrategy, NetSalesGas netSalesGas)
    {
        drainageStrategy.NetSalesGas = netSalesGas;
        return;
        // return await _context.SaveChangesAsync();
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
