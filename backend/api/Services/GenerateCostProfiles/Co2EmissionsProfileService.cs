using api.Context;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Helpers;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services.GenerateCostProfiles;

public class Co2EmissionsProfileService(
    DcdDbContext context,
    ICaseService caseService,
    IDrainageStrategyService drainageStrategyService,
    IProjectWithAssetsRepository projectWithAssetsRepository,
    ITopsideService topsideService)
    : ICo2EmissionsProfileService
{
    public async Task Generate(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(caseId);
        var drainageStrategy = await drainageStrategyService.GetDrainageStrategyWithIncludes(
            caseItem.DrainageStrategyLink,
            d => d.ProductionProfileOil!,
            d => d.AdditionalProductionProfileOil!,
            d => d.ProductionProfileGas!,
            d => d.AdditionalProductionProfileGas!,
            d => d.ProductionProfileWaterInjection!,
            d => d.Co2Emissions!,
            d => d.Co2EmissionsOverride!
        );
        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            return;
        }

        var topside = await topsideService.GetTopsideWithIncludes(caseItem.TopsideLink);
        var project = await projectWithAssetsRepository.GetProjectWithCases(caseItem.ProjectId);

        var fuelConsumptionsProfile = GetFuelConsumptionsProfile(project, caseItem, topside, drainageStrategy);
        var flaringsProfile = GetFlaringsProfile(project, drainageStrategy);
        var lossesProfile = GetLossesProfile(project, drainageStrategy);

        var tempProfile = TimeSeriesCost.MergeCostProfilesList([fuelConsumptionsProfile, flaringsProfile, lossesProfile]);

        var convertedValues = tempProfile.Values.Select(v => v / 1000);

        var newProfile = new TimeSeries<double>
        {
            StartYear = tempProfile.StartYear,
            Values = convertedValues.ToArray(),
        };

        var drillingEmissionsProfile = await CalculateDrillingEmissions(project, caseItem.WellProjectLink);

        var totalProfile =
            TimeSeriesCost.MergeCostProfiles(newProfile, drillingEmissionsProfile);

        if (drainageStrategy.Co2Emissions != null)
        {
            drainageStrategy.Co2Emissions.Values = totalProfile.Values;
            drainageStrategy.Co2Emissions.StartYear = totalProfile.StartYear;
        }
        else
        {
            drainageStrategy.Co2Emissions = new()
            {
                StartYear = totalProfile.StartYear,
                Values = totalProfile.Values
            };
        }
    }


    private static TimeSeriesVolume GetLossesProfile(Project project, DrainageStrategy drainageStrategy)
    {
        var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        var lossesProfile = new TimeSeriesVolume
        {
            StartYear = losses.StartYear,
            Values = losses.Values.Select(loss => loss * project.CO2Vented).ToArray(),
        };
        return lossesProfile;
    }

    private static TimeSeriesVolume GetFlaringsProfile(Project project, DrainageStrategy drainageStrategy)
    {
        var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);

        var flaringsProfile = new TimeSeriesVolume
        {
            StartYear = flarings.StartYear,
            Values = flarings.Values.Select(flare => flare * project.CO2EmissionsFromFlaredGas).ToArray(),
        };
        return flaringsProfile;
    }

    private static TimeSeriesVolume GetFuelConsumptionsProfile(
        Project project,
        Case caseItem,
        Topside topside,
        DrainageStrategy drainageStrategy
    )
    {
        var fuelConsumptions =
            EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        var fuelConsumptionsProfile = new TimeSeriesVolume
        {
            StartYear = fuelConsumptions.StartYear,
            Values = fuelConsumptions.Values.Select(fuel => fuel * project.CO2EmissionFromFuelGas).ToArray(),
        };
        return fuelConsumptionsProfile;
    }

    private async Task<TimeSeriesVolume> CalculateDrillingEmissions(Project project, Guid wellProjectId)
    {
        var linkedWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(w => w.WellProjectId == wellProjectId).ToListAsync();

        var wellDrillingSchedules = new TimeSeries<double>();
        foreach (var linkedWell in linkedWells)
        {
            if (linkedWell.DrillingSchedule == null)
            {
                continue;
            }

            var timeSeries = new TimeSeries<double>
            {
                StartYear = linkedWell.DrillingSchedule.StartYear,
                Values = linkedWell.DrillingSchedule.Values.Select(v => (double)v).ToArray(),
            };
            wellDrillingSchedules = TimeSeriesCost.MergeCostProfiles(wellDrillingSchedules, timeSeries);
        }

        var drillingEmission = new ProductionProfileGas
        {
            StartYear = wellDrillingSchedules.StartYear,
            Values = wellDrillingSchedules.Values
                .Select(well => well * project.AverageDevelopmentDrillingDays * project.DailyEmissionFromDrillingRig)
                .ToArray(),
        };

        return drillingEmission;
    }
}
