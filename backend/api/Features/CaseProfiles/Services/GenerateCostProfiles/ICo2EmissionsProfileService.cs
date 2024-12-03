namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface ICo2EmissionsProfileService
{
    Task Generate(Guid caseId);
}
