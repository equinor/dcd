using api.Adapters;
using api.Context;
using api.Dtos;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class FuelFlaringLossesProfileService : IFuelFlaringLossesProfileService
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public FuelFlaringLossesProfileService(
        DcdDbContext context,
        ICaseService caseService,
        IProjectService projectService,
        ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService,
        IMapper mapper)
    {
        _context = context;
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
        _mapper = mapper;
    }

    public async Task<FuelFlaringAndLossesDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var total = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { fuelConsumptions, flaring, losses });

        var fuelFlaringLosses = drainageStrategy.FuelFlaringAndLosses ?? new FuelFlaringAndLosses();
        fuelFlaringLosses.StartYear = total.StartYear;
        fuelFlaringLosses.Values = total.Values;

        UpdateDrainageStrategyAndSave(drainageStrategy, fuelFlaringLosses);

        var dto = _mapper.Map<FuelFlaringAndLossesDto>(fuelFlaringLosses, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());
        return dto ?? new FuelFlaringAndLossesDto();
    }

    private void UpdateDrainageStrategyAndSave(DrainageStrategy drainageStrategy, FuelFlaringAndLosses fuelFlaringAndLosses)
    {
        drainageStrategy.FuelFlaringAndLosses = fuelFlaringAndLosses;
        // return await _context.SaveChangesAsync();
    }
}
