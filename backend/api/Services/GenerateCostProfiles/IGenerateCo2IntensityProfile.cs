using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2IntensityProfile
    {
        Co2IntensityDto Generate(Guid caseId);
    }
}
