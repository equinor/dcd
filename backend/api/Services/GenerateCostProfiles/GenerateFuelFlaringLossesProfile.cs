using api.Adapters;
using api.Context;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateFuelFlaringLossesProfile : IGenerateFuelFlaringLossesProfile
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;
    private readonly DcdDbContext _context;

    public GenerateFuelFlaringLossesProfile(DcdDbContext context, ICaseService caseService, IProjectService projectService, ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService)
    {
        _context = context;
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public async Task<FuelFlaringAndLossesDto> GenerateAsync(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var total = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { fuelConsumptions, flaring, losses });

        var fuelFlaringLosses = drainageStrategy.FuelFlaringAndLosses ?? new FuelFlaringAndLosses();
        fuelFlaringLosses.StartYear = total.StartYear;
        fuelFlaringLosses.Values = total.Values;

        var saveResult = await UpdateDrainageStrategyAndSaveAsync(drainageStrategy, fuelFlaringLosses);

        var dto = DrainageStrategyDtoAdapter.Convert<FuelFlaringAndLossesDto, FuelFlaringAndLosses>(fuelFlaringLosses, project.PhysicalUnit);
        return dto ?? new FuelFlaringAndLossesDto();
    }

    private async Task<int> UpdateDrainageStrategyAndSaveAsync(DrainageStrategy drainageStrategy, FuelFlaringAndLosses fuelFlaringAndLosses)
    {
        drainageStrategy.FuelFlaringAndLosses = fuelFlaringAndLosses;
        return await _context.SaveChangesAsync();
    }
}
