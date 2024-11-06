using api.Models;

namespace api.Services.EconomicsServices;

public class CalculateNPVService(ICaseService caseService) : ICalculateNPVService
{
    private static TimeSeries<double>? GetCashflowProfile(Case caseItem)
    {
        if (caseItem.CalculatedTotalIncomeCostProfile == null || caseItem.CalculatedTotalCostCostProfile == null)
        {
            return null;
        }

        return EconomicsHelper.CalculateCashFlow(caseItem.CalculatedTotalIncomeCostProfile, caseItem.CalculatedTotalCostCostProfile);
    }

    public async Task CalculateNPV(Guid caseId)
    {
        var caseItem = await caseService.GetCaseWithIncludes(
            caseId,
            c => c.Project,
            c => c.CalculatedTotalIncomeCostProfile!,
            c => c.CalculatedTotalCostCostProfile!

        );

        var cashflowProfile = GetCashflowProfile(caseItem);

        if (cashflowProfile == null)
        {
            caseItem.NPV = 0;
            return;
        }

        var discountRate = caseItem.Project.DiscountRate;

        if (discountRate == 0)
        {
            caseItem.NPV = 0;
            return;
        }

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;

        var npvValue = EconomicsHelper.CalculateDiscountedVolume(
                cashflowProfile.Values,
                discountRate,
                cashflowProfile.StartYear + Math.Abs(nextYearInRelationToDg4Year)
            );

        caseItem.NPV = npvValue / caseItem.Project.ExchangeRateUSDToNOK;
    }
}

