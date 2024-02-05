using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateImportedElectricityProfile
    {
        Task<ImportedElectricityDto> GenerateAsync(Guid caseId);
    }
}
