using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.CaseProfiles.Services;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;

public class ImportedElectricityProfileService(
    ICaseService caseService,
    ITopsideService topsideService,
    IDrainageStrategyService drainageStrategyService)
    : IImportedElectricityProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
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

        var topside = await topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);

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
