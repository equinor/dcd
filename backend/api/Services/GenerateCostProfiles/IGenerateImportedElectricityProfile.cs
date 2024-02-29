using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateImportedElectricityProfile
    {
        Task<ImportedElectricityDto> Generate(Guid caseId);
    }
}
