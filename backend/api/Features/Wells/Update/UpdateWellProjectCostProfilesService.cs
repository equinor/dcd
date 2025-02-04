using api.Context;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateWellProjectCostProfilesService(DcdDbContext context)
{
    public async Task HandleWellProjects(List<Guid> wellIds)
    {
        var uniqueWellProjectIds = await context.DevelopmentWells
            .Where(x => wellIds.Contains(x.WellId))
            .Select(x => x.WellProjectId)
            .Distinct()
            .ToListAsync();

        var wellProjectIdUsedByCases = await context.Cases
            .Where(x => uniqueWellProjectIds.Contains(x.WellProjectId))
            .Select(x => x.WellProjectId)
            .Distinct()
            .ToListAsync();

        foreach (var wellProjectId in wellProjectIdUsedByCases)
        {
            await UpdateWellProjectCostProfiles(wellProjectId);
        }
    }

    public async Task UpdateWellProjectCostProfiles(Guid wellProjectId)
    {
        var wellProject = await context.WellProjects
            .SingleAsync(x => x.Id == wellProjectId);

        var wellIds = await context.DevelopmentWells
            .Where(ew => ew.WellProjectId == wellProject.Id)
            .Select(ew => ew.WellId)
            .ToListAsync();

        await HandleOilProducerCostProfile(wellProject, wellIds);
        await HandleGasProducerCostProfile(wellProject, wellIds);
        await HandleWaterInjectorCostProfile(wellProject, wellIds);
        await HandleGasInjectorCostProfile(wellProject, wellIds);
    }

    private async Task HandleOilProducerCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (oilProducerWells, developmentWellOilProducer) = await GetWellData(wellIds, wellProject.Id, WellCategory.Oil_Producer);
        var oilProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(oilProducerWells, developmentWellOilProducer);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.OilProducerCostProfile))
            .SingleAsync(x => x.WellProjectId == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.OilProducerCostProfile);

        profile.StartYear = oilProducerTimeSeries.StartYear;
        profile.Values = oilProducerTimeSeries.Values;
    }

    private async Task HandleGasProducerCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (gasProducerWells, developmentWellGasProducer) = await GetWellData(wellIds, wellProject.Id, WellCategory.Gas_Producer);
        var gasProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasProducerWells, developmentWellGasProducer);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.GasProducerCostProfile))
            .SingleAsync(x => x.WellProjectId == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.GasProducerCostProfile);

        profile.StartYear = gasProducerTimeSeries.StartYear;
        profile.Values = gasProducerTimeSeries.Values;
    }

    private async Task HandleWaterInjectorCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (waterInjectorWells, developmentWellWaterInjector) = await GetWellData(wellIds, wellProject.Id, WellCategory.Water_Injector);
        var waterInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(waterInjectorWells, developmentWellWaterInjector);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.WaterInjectorCostProfile))
            .SingleAsync(x => x.WellProjectId == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.WaterInjectorCostProfile);

        profile.StartYear = waterInjectorTimeSeries.StartYear;
        profile.Values = waterInjectorTimeSeries.Values;
    }

    private async Task HandleGasInjectorCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (gasInjectorWells, developmentWellGasInjector) = await GetWellData(wellIds, wellProject.Id, WellCategory.Gas_Injector);
        var gasInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasInjectorWells, developmentWellGasInjector);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.GasInjectorCostProfile))
            .SingleAsync(x => x.WellProjectId == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.GasInjectorCostProfile);

        profile.StartYear = gasInjectorTimeSeries.StartYear;
        profile.Values = gasInjectorTimeSeries.Values;
    }

    private async Task<(List<Well>, List<DevelopmentWell>)> GetWellData(List<Guid> wellIds, Guid wellProjectId, WellCategory wellCategory)
    {
        var wells = await context.Wells
            .Where(w => wellIds.Contains(w.Id))
            .Where(w => w.WellCategory == wellCategory)
            .ToListAsync();

        var wellWellIds = wells.Select(x => x.Id).ToList();

        var developmentWells = await context.DevelopmentWells
            .Where(ew => ew.WellProjectId == wellProjectId)
            .Where(ew => wellWellIds.Contains(ew.WellId))
            .ToListAsync();

        return (wells, developmentWells);
    }

    private static TimeSeriesCost GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<DevelopmentWell> developmentWells)
    {
        var costProfilesList = new List<TimeSeriesCost>();

        foreach (var developmentWell in developmentWells)
        {
            if (developmentWell.Values.Length > 0)
            {
                var well = wells.Single(w => w.Id == developmentWell.WellId);
                var values = developmentWell.Values.Select(ds => ds * well.WellCost).ToArray();

                costProfilesList.Add(new TimeSeriesCost
                {
                    Values = values,
                    StartYear = developmentWell.StartYear
                });
            }
        }

        return TimeSeriesMerger.MergeTimeSeries(costProfilesList);
    }
}
