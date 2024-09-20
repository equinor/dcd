using api.Adapters;
using api.Context;
using api.Dtos;
using api.Helpers;
using api.Models;

using AutoMapper;

namespace api.Services.GenerateCostProfiles;

public class ImportedElectricityProfileService : IImportedElectricityProfileService
{
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly ITopsideService _topsideService;

    public ImportedElectricityProfileService(
        ICaseService caseService,
        ITopsideService topsideService,
        IDrainageStrategyService drainageStrategyService
    )
    {
        _caseService = caseService;
        _topsideService = topsideService;
        _drainageStrategyService = drainageStrategyService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await _drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ImportedElectricity!,
            d => d.ImportedElectricityOverride!,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!,
            d => d.ProductionProfileWaterInjection!
        );

        if (drainageStrategy.ImportedElectricityOverride?.Override == true)
        {
            return;
        }

        var topside = await _topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);

        var facilitiesAvailability = caseItem.FacilitiesAvailability;

        var totalUseOfPower = EmissionCalculationHelper.CalculateTotalUseOfPower(topside, drainageStrategy, facilitiesAvailability);

        var calculateImportedElectricity = CalculateImportedElectricity(topside.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var importedElectricity = new ImportedElectricity
        {
            StartYear = calculateImportedElectricity.StartYear,
            Values = calculateImportedElectricity.Values
        };

        if (drainageStrategy.ImportedElectricity != null)
        {
            drainageStrategy.ImportedElectricity.StartYear = importedElectricity.StartYear;
            drainageStrategy.ImportedElectricity.Values = importedElectricity.Values;
        }
        else
        {
            drainageStrategy.ImportedElectricity = importedElectricity;
        }
    }

    private static TimeSeries<double> CalculateImportedElectricity(
        double peakElectricityImported,
        double facilityAvailability,
        TimeSeries<double> totalUseOfPower
    )
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
