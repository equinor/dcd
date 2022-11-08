using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateImportedElectricityProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;

    public GenerateImportedElectricityProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
    }

    public async Task<ImportedElectricityDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProject(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(drainageStrategy);
        var calculateImportedElectricity =
            CalculateImportedElectricity(topside.PeakElectricityImported, fuelConsumptions, flarings, losses);

        var importedElectricity = new ImportedElectricity
        {
            StartYear = calculateImportedElectricity.StartYear,
            Values = calculateImportedElectricity.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(importedElectricity, project.PhysicalUnit);
        return dto ?? new ImportedElectricityDto();
    }

    private static TimeSeries<double> CalculateImportedElectricity(double peakElectricityImported,
        TimeSeries<double> fuelConsumption, TimeSeries<double> flaring,
        TimeSeries<double> losses)
    {
        const int hoursInOneYear = 8766;
        var peakElectricityImportedFromGrid = peakElectricityImported * 1.1;
        var fuelFlaringLosses =
            TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumption, flaring), losses);

        var importedElectricityProfile = new TimeSeriesVolume
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values =
                fuelFlaringLosses.Values
                    .Select(value => value * peakElectricityImportedFromGrid * hoursInOneYear / 1000)
                    .ToArray(),
        };

        return importedElectricityProfile;
    }
}
