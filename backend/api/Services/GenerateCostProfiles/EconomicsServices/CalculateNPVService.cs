

namespace api.Services.EconomicsServices;

public class CalculateNPVService : ICalculateNPVService
{
    private readonly ICaseService _caseService;

    public CalculateNPVService(ICaseService caseService)
    {
        _caseService = caseService;
    }

    public async Task CalculateNPV(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(
                        caseId,
                        c => c.Project
                    );

        var cashflowProfile = caseItem.CalculatedTotalIncomeCostProfile != null && caseItem.CalculatedTotalCostCostProfile != null
            ? EconomicsHelper.CalculateCashFlow(caseItem.CalculatedTotalIncomeCostProfile, caseItem.CalculatedTotalCostCostProfile)
            : null;
        var discountRate = caseItem.Project?.DiscountRate ?? 8;

        var currentYear = DateTime.Now.Year;
        var nextYear = currentYear + 1;
        var dg4Year = caseItem.DG4Date.Year;
        var nextYearInRelationToDg4Year = nextYear - dg4Year;

        var npvValue = cashflowProfile != null && discountRate > 0
            ? EconomicsHelper.CalculateDiscountedVolume(cashflowProfile.Values, discountRate, cashflowProfile.StartYear + Math.Abs(nextYearInRelationToDg4Year))
            : 0;

        caseItem.NPV = npvValue;
    }
}

