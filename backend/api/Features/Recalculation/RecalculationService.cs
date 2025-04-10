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

        //1
        WellCostProfileService.RunCalculation(caseItem);
        RigCostProfileService.RunCalculation(caseItem);
        CessationCostProfileService.RunCalculation(caseItem, developmentWell);
        FuelFlaringLossesProfileService.RunCalculation(caseItem);
        GenerateGAndGAdminCostProfile.RunCalculation(caseItem, explorationWells);
        ImportedElectricityProfileService.RunCalculation(caseItem);
        OpexCostProfileService.RunCalculation(caseItem, developmentWell);
        Co2EmissionsProfileService.RunCalculation(caseItem, developmentWell);
        CondensateProductionProfileService.RunCalculation(caseItem);
        ProductionProfileNglProfileService.RunCalculation(caseItem);
        //2
        StudyCostProfileService.RunCalculation(caseItem);
        NetSaleGasProfileService.RunCalculation(caseItem);
        CalculateTotalOilIncomeService.RunCalculation(caseItem);
        //3
        CalculateTotalCostService.RunCalculation(caseItem);
        CalculateTotalIncomeService.RunCalculation(caseItem);
        CalculateTotalGasIncomeService.RunCalculation(caseItem);
        //4
        CalculateNpvService.RunCalculation(caseItem);
        CalculateBreakEvenOilPriceService.RunCalculation(caseItem);
        CalculateCashflowService.RunCalculation(caseItem);
        TotalExportedVolumesProfileService.RunCalculation(caseItem);
        //5
        CalculatedDiscountedCashflowService.RunCalculation(caseItem);
        Co2IntensityProfileService.RunCalculation(caseItem);
        //6
        AverageCo2IntensityProfileService.RunCalculation(caseItem);
    }
}
