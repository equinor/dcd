using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IImportedElectricityProfileService
    {
        Task<ImportedElectricityDto> Generate(Guid caseId);
    }
}
