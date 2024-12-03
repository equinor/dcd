namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IGenerateGAndGAdminCostProfile
{
    Task Generate(Guid caseId);
}
