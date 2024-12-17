namespace api.Features.Cases.Recalculation.Types.StudyCostProfile;

public interface IStudyCostProfileService
{
    Task Generate(Guid caseId);
}
