using api.Adapters;
using api.Context;
using api.Dtos;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class GenerateImportedElectricityProfile : IGenerateImportedElectricityProfile
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IProjectService _projectService;
    private readonly ITopsideService _topsideService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public GenerateImportedElectricityProfile(
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

    public async Task<ImportedElectricityDto> Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);
        var topside = await _topsideService.GetTopside(caseItem.TopsideLink);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);

        var facilitiesAvailability = caseItem.FacilitiesAvailability;

        var totalUseOfPower = EmissionCalculationHelper.CalculateTotalUseOfPower(topside, drainageStrategy, facilitiesAvailability);

        var calculateImportedElectricity =
            CalculateImportedElectricity(topside.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var importedElectricity = drainageStrategy.ImportedElectricity ?? new ImportedElectricity();

        importedElectricity.StartYear = calculateImportedElectricity.StartYear;
        importedElectricity.Values = calculateImportedElectricity.Values;

        await UpdateDrainageStrategyAndSave(drainageStrategy, importedElectricity);

        var dto = _mapper.Map<ImportedElectricityDto>(importedElectricity);

        return dto ?? new ImportedElectricityDto();
    }

    private async Task<int> UpdateDrainageStrategyAndSave(DrainageStrategy drainageStrategy, ImportedElectricity importedElectricity)
    {
        drainageStrategy.ImportedElectricity = importedElectricity;
        return await _context.SaveChangesAsync();
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
