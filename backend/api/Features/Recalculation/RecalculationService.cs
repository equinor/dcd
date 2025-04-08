using api.Context;
using api.Features.Recalculation.BreakEven;
using api.Features.Recalculation.Co2;
using api.Features.Recalculation.Cost;
using api.Features.Recalculation.Production;
using api.Features.Recalculation.RevenuesAndCashflow;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Recalculation;

public class RecalculationService(DcdDbContext context, RecalculationRepository recalculationRepository)
{
    public async Task SaveChangesAndRecalculateCase(Guid caseId)
    {
        // Saving before RunCalculation ensures that LoadCaseData actually loads all relevant data.
        await context.SaveChangesAsync();

        RunRecalculations(await recalculationRepository.LoadCaseData(caseId));

        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAndRecalculateProject(Guid projectId)
    {
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
    }

    private static void RunRecalculations(CaseWithCampaignWells caseWithCampaignWells)
    {
        var caseItem = caseWithCampaignWells.CaseItem;
        var explorationWells = caseWithCampaignWells.ExplorationWells;
        var developmentWell = caseWithCampaignWells.DevelopmentWells;

        WellCostProfileService.RunCalculation(caseItem);
        RigCostProfileService.RunCalculation(caseItem);
        StudyCostProfileService.RunCalculation(caseItem);
        CessationCostProfileService.RunCalculation(caseItem, developmentWell);
        FuelFlaringLossesProfileService.RunCalculation(caseItem);
        GenerateGAndGAdminCostProfile.RunCalculation(caseItem, explorationWells);
        ImportedElectricityProfileService.RunCalculation(caseItem);
        NetSaleGasProfileService.RunCalculation(caseItem); // TODO look into this
        OpexCostProfileService.RunCalculation(caseItem, developmentWell);
        Co2EmissionsProfileService.RunCalculation(caseItem, developmentWell);
        Co2IntensityProfileService.RunCalculation(caseItem);
        AverageCo2IntensityProfileService.RunCalculation(caseItem);
        CalculateTotalIncomeService.RunCalculation(caseItem);
        CalculateTotalCostService.RunCalculation(caseItem);
        CalculateNpvService.RunCalculation(caseItem);
        CalculateBreakEvenOilPriceService.RunCalculation(caseItem);
        ProductionProfileNglProfileService.RunCalculation(caseItem);
        CondensateProductionProfileService.RunCalculation(caseItem);
        TotalExportedVolumesProfileService.RunCalculation(caseItem);
        CalculateTotalOilIncomeService.RunCalculation(caseItem);
        CalculateTotalGasIncomeService.RunCalculation(caseItem);
        CalculateCashflowService.RunCalculation(caseItem);
        CalculatedDiscountedCashflowService.RunCalculation(caseItem);
    }
}
