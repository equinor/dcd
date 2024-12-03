namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface ICessationCostProfileService
{
    Task Generate(Guid caseId);
}
