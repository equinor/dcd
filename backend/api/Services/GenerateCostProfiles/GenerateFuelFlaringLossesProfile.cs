using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateFuelFlaringLossesProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;

    public GenerateFuelFlaringLossesProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
    }

    public FuelFlaringAndLossesDto GenerateFuelFlaringAndLosses(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(drainageStrategy);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumptions, flarings), losses);


        var fuelFlaringLosses = new FuelFlaringAndLosses
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(fuelFlaringLosses, project.PhysicalUnit);
        return dto;
    }
}
