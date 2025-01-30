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

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationService(DcdDbContext context, IServiceProvider serviceProvider) : IRecalculationService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

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
