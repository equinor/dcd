using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateCo2DrillingFlaringFuelTotals
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;
    private readonly WellProjectService _wellProjectService;

    public GenerateCo2DrillingFlaringFuelTotals(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
    }

    public Co2DrillingFlaringFuelTotalsDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);

        var fuelConsumptionsTotal = GetFuelConsumptionsProfileTotal(project, caseItem, topside, drainageStrategy);
        var flaringsTotal = GetFlaringsProfileTotal(project, drainageStrategy);
        var drillingEmissionsTotal = CalculateDrillingEmissionsTotal(project, wellProject);

        var co2DrillingFlaringFuelTotals = new Co2DrillingFlaringFuelTotals
        {
            Co2Drilling = drillingEmissionsTotal,
            Co2Flaring = flaringsTotal,
            Co2Fuel = fuelConsumptionsTotal,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(co2DrillingFlaringFuelTotals, project.PhysicalUnit);
        return dto ?? new Co2DrillingFlaringFuelTotalsDto();
    }

    private static double GetFlaringsProfileTotal(Project project, DrainageStrategy drainageStrategy)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);

        var flaringsProfile = new TimeSeriesVolume
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * project.CO2EmissionsFromFlaredGas).ToArray(),
        };
        return flaringsProfile.Values.Sum() / 1000;
    }

    private static double GetFuelConsumptionsProfileTotal(Project project, Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        var fuelConsumptionsProfile = new TimeSeriesVolume
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * project.CO2EmissionFromFuelGas).ToArray(),
        };
        return fuelConsumptionsProfile.Values.Sum() / 1000;
    }

    private static double CalculateDrillingEmissionsTotal(Project project, WellProject wellProject)
    {
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory))
            .ToList();
        if (linkedWells == null)
        {
            return 0.0;
        }

        var wellDrillingSchedules = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray(),
            };
            wellDrillingSchedules = TimeSeriesCost.MergeCostProfiles(wellDrillingSchedules, timeSeries);
        }

        var drillingEmission = new ProductionProfileGas
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray(),
        };

        return drillingEmission.Values.Sum();
    }
}
