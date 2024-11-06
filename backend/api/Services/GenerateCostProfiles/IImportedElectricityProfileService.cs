namespace api.Services.GenerateCostProfiles;

public interface IImportedElectricityProfileService
{
    Task Generate(Guid caseId);
}