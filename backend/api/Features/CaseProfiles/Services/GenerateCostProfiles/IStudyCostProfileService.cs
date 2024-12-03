namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IStudyCostProfileService
{
    Task Generate(Guid caseId);
}
