using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateCo2EmissionsProfile
{
    private const double Co2EmissionFromFuelGas = 2.34;
    private const double Co2EmissionFromFlaredGas = 3.74;
    private const double Co2Vented = 1.96;
    private const int AverageDevelopmentWellDrillingDays = 50;
    private const int DailyEmissionFromDrillingRig = 100;
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;
    private readonly WellProjectService _wellProjectService;

    public GenerateCo2EmissionsProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
    }

    public Co2EmissionsDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
        var fuelConsumptionsProfile = GetFuelConsumptionsProfile(caseItem, topside, drainageStrategy);
        var flaringsProfile = GetFlaringsProfile(drainageStrategy);
        var lossesProfile = GetLossesProfile(drainageStrategy);
        var drillingEmissionsProfile = CalculateDrillingEmissions(drainageStrategy, wellProject);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(
                TimeSeriesCost.MergeCostProfiles(fuelConsumptionsProfile, flaringsProfile),
                lossesProfile), drillingEmissionsProfile);
        var co2Emission = new Co2Emissions
        {
            StartYear = totalProfile.StartYear,
            Values = totalProfile.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(co2Emission, project.PhysicalUnit);
        return dto;
    }

    private static TimeSeriesVolume GetLossesProfile(DrainageStrategy drainageStrategy)
    {
        var losses = EmissionCalculationHelper.CalculateLosses(drainageStrategy);

        var lossesProfile = new TimeSeriesVolume
        {
            StartYear = losses.StartYear,
            Values = losses.Values.Select(loss => loss * Co2Vented).ToArray(),
        };
        return lossesProfile;
    }

    private static TimeSeriesVolume GetFlaringsProfile(DrainageStrategy drainageStrategy)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(drainageStrategy);

        var flaringsProfile = new TimeSeriesVolume
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * Co2EmissionFromFlaredGas).ToArray(),
        };
        return flaringsProfile;
    }

    private static TimeSeriesVolume GetFuelConsumptionsProfile(Case caseItem, Topside topside,
        DrainageStrategy drainageStrategy)
    {
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        var fuelConsumptionsProfile = new TimeSeriesVolume
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * Co2EmissionFromFuelGas).ToArray(),
        };
        return fuelConsumptionsProfile;
    }

    private static TimeSeriesVolume CalculateDrillingEmissions(DrainageStrategy drainageStrategy,
        WellProject wellProject)
    {
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory))
            .ToList();
        if (linkedWells == null)
        {
            return new TimeSeriesVolume();
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

        if (drainageStrategy.ProductionProfileGas != null)
        {
            var drillingEmission = new ProductionProfileGas
            {
                StartYear = drainageStrategy.ProductionProfileGas.StartYear,
                Values = wellDrillingSchedules.Values
                    .Select(well => well * AverageDevelopmentWellDrillingDays * DailyEmissionFromDrillingRig / 1000000)
                    .ToArray(),
            };

            return drillingEmission;
        }

        return new TimeSeriesVolume();
    }
}
