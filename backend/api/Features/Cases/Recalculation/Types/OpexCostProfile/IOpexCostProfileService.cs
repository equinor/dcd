namespace api.Features.Cases.Recalculation.Types.OpexCostProfile;

public interface IOpexCostProfileService
{
    Task Generate(Guid caseId);
}
