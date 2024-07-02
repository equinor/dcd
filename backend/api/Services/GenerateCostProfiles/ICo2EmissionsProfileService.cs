using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface ICo2EmissionsProfileService
    {
        Task<Co2EmissionsDto> Generate(Guid caseId);
    }
}
