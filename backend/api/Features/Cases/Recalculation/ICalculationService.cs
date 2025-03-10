namespace api.Features.Cases.Recalculation;

public interface ICalculationService
{
    void RunCalculation(CaseWithCampaignWells caseWithCampaignWells);
}
