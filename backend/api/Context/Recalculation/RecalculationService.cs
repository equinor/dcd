using api.Context.Recalculation.Types;
using api.Features.CaseProfiles.Services.GenerateCostProfiles;
using api.Features.CaseProfiles.Services.GenerateCostProfiles.EconomicsServices;

using Microsoft.EntityFrameworkCore;

namespace api.Context.Recalculation;

public interface IRecalculationService
{
    Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default);
}

public class RecalculationService(
    DcdDbContext context,
    IWellCostProfileService wellCostProfileService,
    IStudyCostProfileService studyCostProfileService,
    ICessationCostProfileService cessationCostProfileService,
    IFuelFlaringLossesProfileService fuelFlaringLossesProfileService,
    IGenerateGAndGAdminCostProfile generateGAndGAdminCostProfile,
    IImportedElectricityProfileService importedElectricityProfileService,
    INetSaleGasProfileService netSaleGasProfileService,
    IOpexCostProfileService opexCostProfileService,
    ICo2EmissionsProfileService co2EmissionsProfileService,
    ICalculateTotalIncomeService calculateTotalIncomeService,
    ICalculateTotalCostService calculateTotalCostService,
    ICalculateNPVService calculateNpvService,
    ICalculateBreakEvenOilPriceService calculateBreakEvenOilPriceService) : IRecalculationService
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public async Task<int> SaveChangesAndRecalculateAsync(Guid caseId, CancellationToken cancellationToken = default)
    {
        await Semaphore.WaitAsync(cancellationToken);

        try
        {
            await DetectChangesAndCalculateEntities(caseId);

            bool saveFailed;
            var result = 0;

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
        var (wells, drillingScheduleIds) = ExplorationAndWellProjectCostService.GetExplorationAndWellProjectCostToRecalculate(context);
        var rerunStudyCost = StudyCostService.ShouldCalculateStudyCost(context);
        var rerunCessationCostProfile = CessationCostService.ShouldCalculateCessationCostProfile(context);
        var rerunFuelFlaringAndLosses = FuelFlaringAndLossesService.ShouldCalculateFuelFlaringAndLosses(context);
        var rerunGAndGAdminCost = GAndGAdminCostService.ShouldCalculateGAndGAdminCost(context);
        var rerunImportedElectricity = ImportedElectricityService.ShouldCalculateImportedElectricity(context);
        var rerunNetSalesGas = NetSalesGasService.ShouldCalculateNetSalesGas(context);
        var rerunOpex = OpexService.ShouldCalculateOpex(context);
        var rerunCo2Emissions = Co2EmissionsService.ShouldCalculateCo2Emissions(context);
        var rerunTotalIncome = TotalIncomeService.ShouldCalculateTotalIncome(context);
        var rerunTotalCost = TotalCostService.ShouldCalculateTotalCost(context);
        var rerunCalculateNpv = NpvService.ShouldCalculateNpv(context);
        var rerunCalculateBreakEven = BreakEvenOilPriceService.ShouldCalculateBreakEvenOilPrice(context);

        await context.SaveChangesAsync(); // TODO: This is a hack to find the updated values in the calculate services. Need to find a better way to do this.

        if (wells.Count != 0 || drillingScheduleIds.Count != 0)
        {
            await wellCostProfileService.UpdateCostProfilesForWellsFromDrillingSchedules(drillingScheduleIds);
            await wellCostProfileService.UpdateCostProfilesForWells(wells);
        }

        if (rerunStudyCost)
        {
            await studyCostProfileService.Generate(caseId);
        }

        if (rerunCessationCostProfile)
        {
            await cessationCostProfileService.Generate(caseId);
        }

        if (rerunFuelFlaringAndLosses)
        {
            await fuelFlaringLossesProfileService.Generate(caseId);
        }

        if (rerunGAndGAdminCost)
        {
            await generateGAndGAdminCostProfile.Generate(caseId);
        }

        if (rerunImportedElectricity)
        {
            await importedElectricityProfileService.Generate(caseId);
        }

        if (rerunNetSalesGas)
        {
            await netSaleGasProfileService.Generate(caseId);
        }

        if (rerunOpex)
        {
            await opexCostProfileService.Generate(caseId);
        }

        if (rerunCo2Emissions)
        {
            await co2EmissionsProfileService.Generate(caseId);
        }

        if (rerunTotalIncome)
        {
            await calculateTotalIncomeService.CalculateTotalIncome(caseId);
        }

        if (rerunTotalCost)
        {
            await calculateTotalCostService.CalculateTotalCost(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateNpv)
        {
            await calculateNpvService.CalculateNPV(caseId);
        }

        if (rerunTotalIncome || rerunTotalCost || rerunCalculateBreakEven)
        {
            await calculateBreakEvenOilPriceService.CalculateBreakEvenOilPrice(caseId);
        }
    }
}
