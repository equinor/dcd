namespace api.Features.Cases.Recalculation.Types.ImportedElectricityProfile;

public interface IImportedElectricityProfileService
{
    Task Generate(Guid caseId);
}
