using api.Context;
using api.Features.Cases.Recalculation.Types.Helpers;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;

public class FuelFlaringLossesProfileService(DcdDbContext context)
{
    public async Task Generate(Guid caseId)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.FuelFlaringAndLosses,
            ProfileTypes.FuelFlaringAndLossesOverride
        };

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => profileTypes.Contains(y.ProfileType)))
            .SingleAsync(x => x.Id == caseId);

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileWaterInjection)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        if (caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride)?.Override == true)
        {
            return;
        }

        var topside = await context.Topsides.SingleAsync(x => x.Id == caseItem.TopsideLink);
        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .SingleAsync(p => p.Id == caseItem.ProjectId);

        var fuelConsumptions = EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);
        var flaring = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var total = CostProfileMerger.MergeCostProfiles([fuelConsumptions, flaring, losses]);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.FuelFlaringAndLosses);

        profile.StartYear = total.StartYear;
        profile.Values = total.Values;
    }
}
