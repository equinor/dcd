using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.NetSaleGasProfile;

public class NetSaleGasProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.FuelFlaringAndLossesOverride,
            ProfileTypes.NetSalesGas,
            ProfileTypes.NetSalesGasOverride,
            ProfileTypes.ProductionProfileOil,
            ProfileTypes.AdditionalProductionProfileOil
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);
        var project = await context.Projects.SingleAsync(p => p.Id == caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, caseItem, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var calculateNetSaleGas = CalculateNetSaleGas(caseItem, drainageStrategy, fuelConsumptions, flarings, losses);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.NetSalesGas);

        profile.StartYear = calculateNetSaleGas.StartYear;
        profile.Values = calculateNetSaleGas.Values;
    }

    private static TimeSeries<double> CalculateNetSaleGas(Case caseItem,
        DrainageStrategy drainageStrategy,
        TimeSeries<double> fuelConsumption,
        TimeSeries<double> flarings,
        TimeSeries<double> losses)
    {
        if (drainageStrategy.ProductionProfileGas == null)
        {
            return new TimeSeries<double>();
        }

        if (drainageStrategy.GasSolution == GasSolution.Injection)
        {
            return new TimeSeries<double>();
        }

        var fuelFlaringLosses = CostProfileMerger.MergeCostProfiles(fuelConsumption, flarings, losses);

        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            fuelFlaringLosses.StartYear = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).StartYear;
            fuelFlaringLosses.Values = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).Values;
        }

        var negativeFuelFlaringLosses = new TimeSeriesVolume
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray()
        };

        var additionalProductionProfileGas = drainageStrategy.AdditionalProductionProfileGas ?? new TimeSeries<double>();

        var gasProduction = CostProfileMerger.MergeCostProfiles(drainageStrategy.ProductionProfileGas, additionalProductionProfileGas);
        return CostProfileMerger.MergeCostProfiles(gasProduction, negativeFuelFlaringLosses);
    }
}
