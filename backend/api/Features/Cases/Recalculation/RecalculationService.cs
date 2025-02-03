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

public class RecalculationService(DcdDbContext context, RecalculationRepository recalculationRepository, RecalculationDeterminerService recalculationDeterminerService)
{
    public async Task SaveChangesAndRecalculateCase(Guid caseId)
    {
        var originalLazyLoadingEnabled = context.ChangeTracker.LazyLoadingEnabled;
        context.ChangeTracker.LazyLoadingEnabled = false;

        var caseNeedsRecalculation = CaseNeedsRecalculation();
        await context.SaveChangesAsync();

        if (caseNeedsRecalculation)
        {
            RunRecalculations(await recalculationRepository.LoadCaseData(caseId));

            await context.SaveChangesAsync();
        }

        context.ChangeTracker.LazyLoadingEnabled = originalLazyLoadingEnabled;
    }

    public async Task SaveChangesAndRecalculateProject(Guid projectId)
    {
        var originalLazyLoadingEnabled = context.ChangeTracker.LazyLoadingEnabled;
        context.ChangeTracker.LazyLoadingEnabled = false;

        var caseIds = await context.Cases
            .Where(x => x.ProjectId == projectId)
            .Select(x => x.Id)
            .ToListAsync();

        var caseData = await recalculationRepository.LoadCaseData(caseIds);

        foreach (var caseWithDrillingSchedules in caseData)
        {
            RunRecalculations(caseWithDrillingSchedules);
        }

        await context.SaveChangesAsync();

        context.ChangeTracker.LazyLoadingEnabled = originalLazyLoadingEnabled;
    }

    private bool CaseNeedsRecalculation()
    {
        return recalculationDeterminerService.CalculateExplorationAndWellProjectCost()
               || recalculationDeterminerService.CalculateStudyCost()
               || recalculationDeterminerService.CalculateCessationCostProfile()
               || recalculationDeterminerService.CalculateFuelFlaringAndLosses()
               || recalculationDeterminerService.CalculateGAndGAdminCost()
               || recalculationDeterminerService.CalculateImportedElectricity()
               || recalculationDeterminerService.CalculateNetSalesGas()
               || recalculationDeterminerService.CalculateOpex()
               || recalculationDeterminerService.CalculateCo2Intensity()
               || recalculationDeterminerService.CalculateCo2Emissions()
               || recalculationDeterminerService.CalculateTotalIncome()
               || recalculationDeterminerService.CalculateTotalCost()
               || recalculationDeterminerService.CalculateNpv()
               || recalculationDeterminerService.CalculateBreakEvenOilPrice();
    }

    private static void RunRecalculations(CaseWithDrillingSchedules caseWithDrillingSchedules)
    {
        var caseItem = caseWithDrillingSchedules.CaseItem;
        var drillingSchedulesForExplorationWell = caseWithDrillingSchedules.DrillingSchedulesForExplorationWell;
        var drillingSchedulesForDevelopmentWell = caseWithDrillingSchedules.DrillingSchedulesForDevelopmentWell;

        DevelopmentWellCostProfileService.RunCalculation(caseItem);
        ExplorationWellCostProfileService.RunCalculation(caseItem);
        StudyCostProfileService.RunCalculation(caseItem);
        CessationCostProfileService.RunCalculation(caseItem, drillingSchedulesForDevelopmentWell);
        FuelFlaringLossesProfileService.RunCalculation(caseItem);
        GenerateGAndGAdminCostProfile.RunCalculation(caseItem, drillingSchedulesForExplorationWell);
        ImportedElectricityProfileService.RunCalculation(caseItem);
        NetSaleGasProfileService.RunCalculation(caseItem);
        OpexCostProfileService.RunCalculation(caseItem, drillingSchedulesForDevelopmentWell);
        Co2EmissionsProfileService.RunCalculation(caseItem, drillingSchedulesForDevelopmentWell);
        Co2IntensityProfileService.RunCalculation(caseItem);
        CalculateTotalIncomeService.RunCalculation(caseItem);
        CalculateTotalCostService.RunCalculation(caseItem);
        CalculateNpvService.RunCalculation(caseItem);
        CalculateBreakEvenOilPriceService.RunCalculation(caseItem);
    }
}
