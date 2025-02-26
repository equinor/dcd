using api.Context;
using api.Features.Cases.Recalculation.Calculators.CalculateBreakEvenOilPrice;
using api.Features.Cases.Recalculation.Calculators.CalculateNpv;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalCost;
using api.Features.Cases.Recalculation.Calculators.CalculateTotalIncome;
using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Cases.Recalculation.Types.CessationCostProfile;
using api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;
using api.Features.Cases.Recalculation.Types.CondensateProduction;
using api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;
using api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;
using api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;
using api.Features.Cases.Recalculation.Types.NetSaleGasProfile;
using api.Features.Cases.Recalculation.Types.OpexCostProfile;
using api.Features.Cases.Recalculation.Types.ProductionProfileNGL;
using api.Features.Cases.Recalculation.Types.RigCostProfile;
using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Features.Cases.Recalculation.Types.WellCostProfile;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationService(DcdDbContext context, RecalculationRepository recalculationRepository)
{
    public async Task SaveChangesAndRecalculateCase(Guid caseId)
    {
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

    private static void RunRecalculations(CaseWithDrillingSchedules caseWithDrillingSchedules)
    {
        var caseItem = caseWithDrillingSchedules.CaseItem;
        var explorationWells = caseWithDrillingSchedules.ExplorationWells;
        var developmentWell = caseWithDrillingSchedules.DevelopmentWells;

        DevelopmentWellCostProfileService.RunCalculation(caseItem);
        ExplorationWellCostProfileService.RunCalculation(caseItem);
        RigCostProfileService.RunCalculation(caseItem);
        StudyCostProfileService.RunCalculation(caseItem);
        CessationCostProfileService.RunCalculation(caseItem, developmentWell);
        FuelFlaringLossesProfileService.RunCalculation(caseItem);
        GenerateGAndGAdminCostProfile.RunCalculation(caseItem, explorationWells);
        ImportedElectricityProfileService.RunCalculation(caseItem);
        NetSaleGasProfileService.RunCalculation(caseItem);
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
    }
}
