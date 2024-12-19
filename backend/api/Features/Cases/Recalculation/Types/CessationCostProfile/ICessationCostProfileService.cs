namespace api.Features.Cases.Recalculation.Types.CessationCostProfile;

public interface ICessationCostProfileService
{
    Task Generate(Guid caseId);
}
