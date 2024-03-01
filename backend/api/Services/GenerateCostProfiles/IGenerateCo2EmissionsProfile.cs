using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2EmissionsProfile
    {
        Task<Co2EmissionsDto> Generate(Guid caseId);
    }
}
