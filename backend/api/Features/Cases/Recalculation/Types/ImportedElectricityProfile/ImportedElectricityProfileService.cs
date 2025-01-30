using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;

public class ImportedElectricityProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.ImportedElectricity,
            ProfileTypes.ImportedElectricityOverride,
            ProfileTypes.ProductionProfileOil,
            ProfileTypes.AdditionalProductionProfileOil,
            ProfileTypes.ProductionProfileGas,
            ProfileTypes.AdditionalProductionProfileGas,
            ProfileTypes.ProductionProfileWaterInjection
        };

        var caseItem = await context.Cases
            .Include(x => x.Topside)
            .SingleAsync(x => x.Id == caseId);

        await context.TimeSeriesProfiles
            .Where(x => x.CaseId == caseId)
            .Where(x => profileTypes.Contains(x.ProfileType))
            .LoadAsync();

        RunCalculation(caseItem);
    }

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
