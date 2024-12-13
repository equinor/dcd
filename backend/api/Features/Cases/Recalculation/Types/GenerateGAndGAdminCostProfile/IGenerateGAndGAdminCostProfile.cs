namespace api.Features.Cases.Recalculation.Types.GenerateGAndGAdminCostProfile;

public interface IGenerateGAndGAdminCostProfile
{
    Task Generate(Guid caseId);
}
