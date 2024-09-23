using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface ICo2EmissionsProfileService
    {
        Task Generate(Guid caseId);
    }
}
