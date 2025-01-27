using api.Context;
using api.Features.Profiles;
using api.Features.TimeSeriesCalculators;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.Update;

public class UpdateWellProjectCostProfilesService(DcdDbContext context)
{
    public async Task HandleWellProjects(List<Guid> wellIds)
    {
        var uniqueWellProjectIds = await context.WellProjectWell
            .Where(x => wellIds.Contains(x.WellId))
            .Select(x => x.WellProjectId)
            .Distinct()
            .ToListAsync();

        var wellProjectLinksUsedByCases = await context.Cases
            .Where(x => uniqueWellProjectIds.Contains(x.WellProjectLink))
            .Select(x => x.WellProjectLink)
            .Distinct()
            .ToListAsync();

        foreach (var wellProjectLinks in wellProjectLinksUsedByCases)
        {
            await UpdateWellProjectCostProfiles(wellProjectLinks);
        }
    }

    public async Task UpdateWellProjectCostProfiles(Guid wellProjectId)
    {
        var wellProject = await context.WellProjects
            .SingleAsync(x => x.Id == wellProjectId);

        var wellIds = await context.WellProjectWell
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
        var (oilProducerWells, wellProjectWellOilProducer) = await GetWellData(wellIds, wellProject.Id, WellCategory.Oil_Producer);
        var oilProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(oilProducerWells, wellProjectWellOilProducer);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.OilProducerCostProfile))
            .SingleAsync(x => x.WellProjectLink == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.OilProducerCostProfile);

        profile.StartYear = oilProducerTimeSeries.StartYear;
        profile.Values = oilProducerTimeSeries.Values;
    }

    private async Task HandleGasProducerCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (gasProducerWells, wellProjectWellGasProducer) = await GetWellData(wellIds, wellProject.Id, WellCategory.Gas_Producer);
        var gasProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasProducerWells, wellProjectWellGasProducer);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.GasProducerCostProfile))
            .SingleAsync(x => x.WellProjectLink == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.GasProducerCostProfile);

        profile.StartYear = gasProducerTimeSeries.StartYear;
        profile.Values = gasProducerTimeSeries.Values;
    }

    private async Task HandleWaterInjectorCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (waterInjectorWells, wellProjectWellWaterInjector) = await GetWellData(wellIds, wellProject.Id, WellCategory.Water_Injector);
        var waterInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(waterInjectorWells, wellProjectWellWaterInjector);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.WaterInjectorCostProfile))
            .SingleAsync(x => x.WellProjectLink == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.WaterInjectorCostProfile);

        profile.StartYear = waterInjectorTimeSeries.StartYear;
        profile.Values = waterInjectorTimeSeries.Values;
    }

    private async Task HandleGasInjectorCostProfile(WellProject wellProject, List<Guid> wellIds)
    {
        var (gasInjectorWells, wellProjectWellGasInjector) = await GetWellData(wellIds, wellProject.Id, WellCategory.Gas_Injector);
        var gasInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasInjectorWells, wellProjectWellGasInjector);

        var caseItem = await context.Cases
            .Include(x => x.TimeSeriesProfiles.Where(y => y.ProfileType == ProfileTypes.GasInjectorCostProfile))
            .SingleAsync(x => x.WellProjectLink == wellProject.Id);

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.GasInjectorCostProfile);

        profile.StartYear = gasInjectorTimeSeries.StartYear;
        profile.Values = gasInjectorTimeSeries.Values;
    }

    private async Task<(List<Well>, List<WellProjectWell>)> GetWellData(List<Guid> wellIds, Guid wellProjectId, WellCategory wellCategory)
    {
        var wells = await context.Wells
            .Where(w => wellIds.Contains(w.Id))
            .Where(w => w.WellCategory == wellCategory)
            .ToListAsync();

        var wellWellIds = wells.Select(x => x.Id).ToList();

        var wellProjectWells = await context.WellProjectWell
            .Include(wpw => wpw.DrillingSchedule)
            .Where(ew => ew.WellProjectId == wellProjectId)
            .Where(ew => wellWellIds.Contains(ew.WellId))
            .ToListAsync();

        return (wells, wellProjectWells);
    }

    private static TimeSeries<double> GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(List<Well> wells, List<WellProjectWell> wellProjectWells)
    {
        var costProfilesList = new List<TimeSeries<double>?>();

        foreach (var wellProjectWell in wellProjectWells)
        {
            if (wellProjectWell.DrillingSchedule?.Values.Length > 0)
            {
                var well = wells.Single(w => w.Id == wellProjectWell.WellId);
                var values = wellProjectWell.DrillingSchedule.Values.Select(ds => ds * well.WellCost).ToArray();

                costProfilesList.Add(new TimeSeries<double>
                {
                    Values = values,
                    StartYear = wellProjectWell.DrillingSchedule.StartYear,
                });
            }
        }

        return CostProfileMerger.MergeCostProfiles(costProfilesList);
    }
}
