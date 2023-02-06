using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2EmissionsProfile
    {
        Co2EmissionsDto Generate(Guid caseId);
    }
}
