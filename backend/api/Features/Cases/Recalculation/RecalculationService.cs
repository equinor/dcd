using System.Diagnostics;

using api.Context;
using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Cases.Recalculation.Types.CessationCostProfile;
using api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;
using api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;
using api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;
using api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;
using api.Features.Cases.Recalculation.Types.NetSaleGasProfile;
using api.Features.Cases.Recalculation.Types.OpexCostProfile;
using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Features.Cases.Recalculation.Types.WellCostProfile;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationService(DcdDbContext context, IServiceProvider serviceProvider) : IRecalculationService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public async Task<Dictionary<Guid, Dictionary<string, long>>> RunAllRecalculations(List<Guid> caseIds)
    {
        var debugLogForProject = new Dictionary<Guid, Dictionary<string, long>>();

        var (caseItems, loadCaseMs, loadProfilesMs) = await LoadCaseData(caseIds);

        debugLogForProject.Add(Guid.Empty, new Dictionary<string, long>
        {
            { "Load cases", loadCaseMs },
            { "Load profiles", loadProfilesMs }
        });

        foreach (var caseItem in caseItems)
        {
            var debugLogForCase = new Dictionary<string, long>();
            var stopwatch = Stopwatch.StartNew();

            debugLogForCase.Add("Load time series profiles", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            var drillingSchedulesForWellProjectWell = await context.WellProjectWell
                .Where(w => w.WellProjectId == caseItem.WellProjectLink)
                .Select(x => x.DrillingSchedule)
                .Where(x => x != null)
                .Select(x => x!)
                .ToListAsync();

            debugLogForCase.Add("load drilling schedules for well project well", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            var drillingSchedulesForExplorationWell = await context.ExplorationWell
                .Where(w => w.ExplorationId == caseItem.ExplorationLink)
                .Select(x => x.DrillingSchedule)
                .Where(x => x != null)
                .Select(x => x!)
                .ToListAsync();

            debugLogForCase.Add("load drilling schedules for exploration well", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWells(caseItem.Id);
            debugLogForCase.Add("UpdateCostProfilesForWells", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            StudyCostProfileService.RunCalculation(caseItem);
            CessationCostProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            FuelFlaringLossesProfileService.RunCalculation(caseItem);
            GenerateGAndGAdminCostProfile.RunCalculation(caseItem, drillingSchedulesForExplorationWell);
            ImportedElectricityProfileService.RunCalculation(caseItem);
            NetSaleGasProfileService.RunCalculation(caseItem);
            OpexCostProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            Co2EmissionsProfileService.RunCalculation(caseItem, drillingSchedulesForWellProjectWell);
            Co2IntensityProfileService.RunCalculation(caseItem);
            CalculateTotalIncomeService.RunCalculation(caseItem);
            CalculateTotalCostService.RunCalculation(caseItem);
            CalculateNpvService.RunCalculation(caseItem);
            CalculateBreakEvenOilPriceService.RunCalculation(caseItem);

            debugLogForCase.Add("Run calculations", stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

            await context.SaveChangesAsync();

            debugLogForCase.Add("SaveChangesAsync", stopwatch.ElapsedMilliseconds);

            debugLogForProject.Add(caseItem.Id, debugLogForCase);
        }

        return debugLogForProject;
    }

    private async Task<(List<Case>, long, long)> LoadCaseData(List<Guid> caseIds)
    {
        var stopwatch = Stopwatch.StartNew();

        var caseItems = await context.Cases
            .Include(x => x.Project).ThenInclude(x => x.DevelopmentOperationalWellCosts)
            .Include(x => x.Surf)
            .Include(x => x.Topside)
            .Include(x => x.DrainageStrategy)
            .Where(x => caseIds.Contains(x.Id))
            .ToListAsync();

        var loadItemsMs = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .LoadAsync();

        var loadTimeSeriesMs = stopwatch.ElapsedMilliseconds;

        return (caseItems, loadItemsMs, loadTimeSeriesMs);
    }

    public async Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            await DetectChangesAndCalculateEntities(caseId);

            bool saveFailed;
            int result = 0;

            do
            {
                saveFailed = false;
                try
                {
                    result = await context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Iterate over all the entries in the exception
                    foreach (var entry in ex.Entries)
                    {
                        var databaseEntry = entry.GetDatabaseValues();

                        if (databaseEntry == null)
                        {
                            throw new Exception("The entity being updated has been deleted.");
                        }
                        else
                        {
                            var databaseValues = databaseEntry.ToObject();

                            // TODO: Decide how to handle the conflict
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }
            } while (saveFailed);

            return result;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private async Task DetectChangesAndCalculateEntities(Guid caseId)
    {
        var recalculationDeterminerService = serviceProvider.GetRequiredService<RecalculationDeterminerService>();

        var (wells, drillingScheduleIds) = recalculationDeterminerService.CalculateExplorationAndWellProjectCost();
        var rerunStudyCost = recalculationDeterminerService.CalculateStudyCost();
        var rerunCessationCostProfile = recalculationDeterminerService.CalculateCessationCostProfile();
        var rerunFuelFlaringAndLosses = recalculationDeterminerService.CalculateFuelFlaringAndLosses();
        var rerunGAndGAdminCost = recalculationDeterminerService.CalculateGAndGAdminCost();
        var rerunImportedElectricity = recalculationDeterminerService.CalculateImportedElectricity();
        var rerunNetSalesGas = recalculationDeterminerService.CalculateNetSalesGas();
        var rerunOpex = recalculationDeterminerService.CalculateOpex();
        var rerunCo2Intensity = recalculationDeterminerService.CalculateCo2Intensity();
        var rerunCo2Emissions = recalculationDeterminerService.CalculateCo2Emissions();
        var rerunTotalIncome = recalculationDeterminerService.CalculateTotalIncome();
        var rerunTotalCost = recalculationDeterminerService.CalculateTotalCost();
        var rerunCalculateNpv = recalculationDeterminerService.CalculateNpv();
        var rerunCalculateBreakEven = recalculationDeterminerService.CalculateBreakEvenOilPrice();

        await context.SaveChangesAsync(); // TODO: This is a hack to find the updated values in the calculate services. Need to find a better way to do this.
        if (wells.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            await serviceProvider.GetRequiredService<WellCostProfileService>().UpdateCostProfilesForWells(wells);
        }
        if (rerunStudyCost)
        {
            await serviceProvider.GetRequiredService<StudyCostProfileService>().Generate(caseId);
        }

        if (rerunCessationCostProfile)
        {
            await serviceProvider.GetRequiredService<CessationCostProfileService>().Generate(caseId);
        }

        if (rerunFuelFlaringAndLosses)
        {
            await serviceProvider.GetRequiredService<FuelFlaringLossesProfileService>().Generate(caseId);
        }

        if (rerunGAndGAdminCost)
        {
            await serviceProvider.GetRequiredService<GenerateGAndGAdminCostProfile>().Generate(caseId);
        }

        if (rerunImportedElectricity)
        {
            await serviceProvider.GetRequiredService<ImportedElectricityProfileService>().Generate(caseId);
        }

        if (rerunNetSalesGas)
        {
            await serviceProvider.GetRequiredService<NetSaleGasProfileService>().Generate(caseId);
        }

        if (rerunOpex)
        {
            await serviceProvider.GetRequiredService<OpexCostProfileService>().Generate(caseId);
        }

        if (rerunCo2Emissions)
        {
            await serviceProvider.GetRequiredService<Co2EmissionsProfileService>().Generate(caseId);
            await serviceProvider.GetRequiredService<Co2IntensityProfileService>().CalculateCo2IntensityProfile(caseId);
        }

        if (rerunCo2Intensity)
        {
            await serviceProvider.GetRequiredService<Co2IntensityProfileService>().CalculateCo2IntensityProfile(caseId);
        }

        if (rerunTotalIncome)
        {
            var calculateIncomeHelper = serviceProvider.GetRequiredService<CalculateTotalIncomeService>();
            await calculateIncomeHelper.CalculateTotalIncome(caseId);
        }

        if (rerunTotalCost)
        {
            var calculateCostHelper = serviceProvider.GetRequiredService<CalculateTotalCostService>();
            await calculateCostHelper.CalculateTotalCost(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateNpv)
        {
            var calculateNpvHelper = serviceProvider.GetRequiredService<CalculateNpvService>();
            await calculateNpvHelper.CalculateNpv(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateBreakEven)
        {
            var calculateBreakEvenHelper = serviceProvider.GetRequiredService<CalculateBreakEvenOilPriceService>();
            await calculateBreakEvenHelper.CalculateBreakEvenOilPrice(caseId);
        }
    }
}
