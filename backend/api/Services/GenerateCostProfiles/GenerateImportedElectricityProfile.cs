using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateImportedElectricityProfile : IGenerateImportedElectricityProfile
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;

    public GenerateImportedElectricityProfile(ICaseService caseService, IProjectService projectService, ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService)
    {
        _caseService = caseService;
        _projectService = projectService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public ImportedElectricityDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var facilitiesAvailability = caseItem.FacilitiesAvailability;

        var totalUseOfPower = EmissionCalculationHelper.CalculateTotalUseOfPower(topside, drainageStrategy, facilitiesAvailability);

        var calculateImportedElectricity =
            CalculateImportedElectricity(topside.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var importedElectricity = new ImportedElectricity
        {
            StartYear = calculateImportedElectricity.StartYear,
            Values = calculateImportedElectricity.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert<ImportedElectricityDto, ImportedElectricity>(importedElectricity, project.PhysicalUnit);
        return dto ?? new ImportedElectricityDto();
    }

    private static TimeSeries<double> CalculateImportedElectricity(double peakElectricityImported, double facilityAvailability,
        TimeSeries<double> totalUseOfPower)
    {
        const int hoursInOneYear = 8766;
        var peakElectricityImportedFromGrid = peakElectricityImported * 1.1;

        var importedElectricityProfile = new TimeSeriesVolume
        {
            StartYear = totalUseOfPower.StartYear,
            Values =
                totalUseOfPower.Values
                    .Select(value => peakElectricityImportedFromGrid * facilityAvailability * hoursInOneYear * value / 1000)
                    .ToArray(),
        };

        return importedElectricityProfile;
    }
}
