using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateImportedElectricityProfile
    {
        ImportedElectricityDto Generate(Guid caseId);
    }
}
