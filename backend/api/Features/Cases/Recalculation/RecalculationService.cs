using api.Context;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.Recalculation;

public class RecalculationService(
        DcdDbContext context,
        RecalculationRepository recalculationRepository,
        IEnumerable<ICalculationService> calculationServices
    )
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

    private void RunRecalculations(CaseWithCampaignWells caseWithCampaignWells)
    {
        foreach (var service in calculationServices)
        {
            service.RunCalculation(caseWithCampaignWells);
        }
    }
}
