using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
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
            ProfileTypes.ProductionProfileOil
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride)?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);

        var facilitiesAvailability = caseItem.FacilitiesAvailability;

        var totalUseOfPower = EmissionCalculationHelper.CalculateTotalUseOfPower(caseItem, topside, drainageStrategy, facilitiesAvailability);

        var calculateImportedElectricity = CalculateImportedElectricity(topside.PeakElectricityImported, facilitiesAvailability, totalUseOfPower);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.ImportedElectricity);

        profile.StartYear = calculateImportedElectricity.StartYear;
        profile.Values = calculateImportedElectricity.Values;
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
