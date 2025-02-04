using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;

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

        var calculateImportedElectricity = CalculateImportedElectricity(caseItem.Topside!.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.ImportedElectricity);

        profile.StartYear = calculateImportedElectricity.StartYear;
        profile.Values = calculateImportedElectricity.Values;
    }

    private static TimeSeriesCost CalculateImportedElectricity(double peakElectricityImported, double facilityAvailability, TimeSeriesCost totalUseOfPower)
    {
        const int hoursInOneYear = 8766;
        var peakElectricityImportedFromGrid = peakElectricityImported * 1.1;

        return new TimeSeriesCost
        {
            StartYear = totalUseOfPower.StartYear,
            Values = totalUseOfPower.Values
                    .Select(value => peakElectricityImportedFromGrid * facilityAvailability / 100 * hoursInOneYear * value / 1000)
                    .ToArray(),
        };
    }
}
