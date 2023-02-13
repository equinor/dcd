using api.Adapters;
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

    public GenerateFuelFlaringLossesProfile(ICaseService caseService, IProjectService projectService, ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService)
    {
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public FuelFlaringAndLossesDto Generate(Guid caseId)
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

        var fuelFlaringLosses = new FuelFlaringAndLosses
        {
            StartYear = total.StartYear,
            Values = total.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert<FuelFlaringAndLossesDto, FuelFlaringAndLosses>(fuelFlaringLosses, project.PhysicalUnit);
        return dto ?? new FuelFlaringAndLossesDto();
    }
}
