namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IImportedElectricityProfileService
{
    Task Generate(Guid caseId);
}
