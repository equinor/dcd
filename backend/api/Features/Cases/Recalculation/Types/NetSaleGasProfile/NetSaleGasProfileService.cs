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
            ProfileTypes.FuelFlaringAndLossesOverride
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.NetSalesGas)
            .Include(d => d.NetSalesGasOverride)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (drainageStrategy.NetSalesGasOverride?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);
        var project = await context.Projects.SingleAsync(p => p.Id == caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var calculateNetSaleGas = CalculateNetSaleGas(caseItem, drainageStrategy, fuelConsumptions, flarings, losses);

        var netSaleGas = new NetSalesGas
        {
            StartYear = calculateNetSaleGas.StartYear,
            Values = calculateNetSaleGas.Values
        };

        if (drainageStrategy.NetSalesGas != null)
        {
            drainageStrategy.NetSalesGas.StartYear = netSaleGas.StartYear;
            drainageStrategy.NetSalesGas.Values = netSaleGas.Values;
        }
        else
        {
            drainageStrategy.NetSalesGas = new NetSalesGas
            {
                StartYear = netSaleGas.StartYear,
                Values = netSaleGas.Values
            };
        }
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
