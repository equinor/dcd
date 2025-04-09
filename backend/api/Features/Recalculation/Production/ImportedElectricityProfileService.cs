using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Recalculation.Helpers;
using api.Models;

namespace api.Features.Recalculation.Production;

// dependency order 1
public static class ImportedElectricityProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride)?.Override == true)
        {
            return;
        }

        var facilitiesAvailability = caseItem.FacilitiesAvailability;

        var totalUseOfPower = EmissionCalculationHelper.CalculateTotalUseOfPower(caseItem, facilitiesAvailability);

        var calculateImportedElectricity = CalculateImportedElectricity(caseItem.Topside.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.ImportedElectricity);

        profile.StartYear = calculateImportedElectricity.StartYear;
        profile.Values = calculateImportedElectricity.Values;
    }

    private static TimeSeries CalculateImportedElectricity(double peakElectricityImported, double facilityAvailability, TimeSeries totalUseOfPower)
    {
        const int hoursInOneYear = 8766;
        var peakElectricityImportedFromGrid = peakElectricityImported * 1.1;

        return new TimeSeries
        {
            StartYear = totalUseOfPower.StartYear,
            Values = totalUseOfPower.Values
                .Select(value => peakElectricityImportedFromGrid * facilityAvailability / 100 * hoursInOneYear * value / 1000)
                .ToArray()
        };
    }
}
