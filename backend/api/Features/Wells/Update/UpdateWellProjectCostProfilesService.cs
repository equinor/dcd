using api.Context;
using api.Exceptions;
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
        var wellProject = await context.WellProjects.SingleOrDefaultAsync(x => x.Id == wellProjectId)
                          ?? throw new NotFoundInDbException($"WellProject {wellProjectId} not found in database.");

        var wellIds = await context.WellProjectWell
            .Where(ew => ew.WellProjectId == wellProject.Id)
            .Select(ew => ew.WellId)
            .ToListAsync();

        var (oilProducerWells, wellProjectWellOilProducer) = await GetWellData(wellIds, wellProjectId, WellCategory.Oil_Producer);
        var oilProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(oilProducerWells, wellProjectWellOilProducer);
        var oilProducerCostProfile = new OilProducerCostProfile
        {
            Values = oilProducerTimeSeries.Values,
            StartYear = oilProducerTimeSeries.StartYear
        };

        var (gasProducerWells, wellProjectWellGasProducer) = await GetWellData(wellIds, wellProjectId, WellCategory.Gas_Producer);
        var gasProducerTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasProducerWells, wellProjectWellGasProducer);
        var gasProducerCostProfile = new GasProducerCostProfile
        {
            Values = gasProducerTimeSeries.Values,
            StartYear = gasProducerTimeSeries.StartYear
        };

        var (waterInjectorWells, wellProjectWellWaterInjector) = await GetWellData(wellIds, wellProjectId, WellCategory.Water_Injector);
        var waterInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(waterInjectorWells, wellProjectWellWaterInjector);
        var waterInjectorCostProfile = new WaterInjectorCostProfile
        {
            Values = waterInjectorTimeSeries.Values,
            StartYear = waterInjectorTimeSeries.StartYear
        };

        var (gasInjectorWells, wellProjectWellGasInjector) = await GetWellData(wellIds, wellProjectId, WellCategory.Gas_Injector);
        var gasInjectorTimeSeries = GenerateWellProjectCostProfileFromDrillingSchedulesAndWellCost(gasInjectorWells, wellProjectWellGasInjector);
        var gasInjectorCostProfile = new GasInjectorCostProfile
        {
            Values = gasInjectorTimeSeries.Values,
            StartYear = gasInjectorTimeSeries.StartYear
        };

        wellProject.OilProducerCostProfile = oilProducerCostProfile;
        wellProject.GasProducerCostProfile = gasProducerCostProfile;
        wellProject.WaterInjectorCostProfile = waterInjectorCostProfile;
        wellProject.GasInjectorCostProfile = gasInjectorCostProfile;
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
