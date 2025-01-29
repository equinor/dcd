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
            ProfileTypes.AdditionalProductionProfileOil,
            ProfileTypes.ProductionProfileGas,
            ProfileTypes.AdditionalProductionProfileGas,
            ProfileTypes.ProductionProfileWaterInjection
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);
        var project = await context.Projects.SingleAsync(p => p.Id == caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, caseItem, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, caseItem, drainageStrategy);

        var calculateNetSaleGas = CalculateNetSaleGas(caseItem, drainageStrategy, fuelConsumptions, flarings, losses);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.NetSalesGas);

        profile.StartYear = calculateNetSaleGas.StartYear;
        profile.Values = calculateNetSaleGas.Values;
    }

    private static TimeSeriesCost CalculateNetSaleGas(Case caseItem,
        DrainageStrategy drainageStrategy,
        TimeSeriesCost fuelConsumption,
        TimeSeriesCost flarings,
        TimeSeriesCost losses)
    {
        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas) == null)
        {
            return new TimeSeriesCost();
        }

        if (drainageStrategy.GasSolution == GasSolution.Injection)
        {
            return new TimeSeriesCost();
        }

        var fuelFlaringLosses = CostProfileMerger.MergeCostProfiles(fuelConsumption, flarings, losses);

        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            fuelFlaringLosses.StartYear = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).StartYear;
            fuelFlaringLosses.Values = caseItem.GetProfile(ProfileTypes.FuelFlaringAndLossesOverride).Values;
        }

        var negativeFuelFlaringLosses = new TimeSeriesCost
        {
            StartYear = fuelFlaringLosses.StartYear,
            Values = fuelFlaringLosses.Values.Select(x => x * -1).ToArray()
        };

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        var additionalProductionProfileGas = new TimeSeriesCost(additionalProductionProfileGasProfile);

        var productionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas);
        var productionProfileGasTimeSeries = new TimeSeriesCost(productionProfileGasProfile);

        var gasProduction = CostProfileMerger.MergeCostProfiles(productionProfileGasTimeSeries, additionalProductionProfileGas);
        return CostProfileMerger.MergeCostProfiles(gasProduction, negativeFuelFlaringLosses);
    }
}
