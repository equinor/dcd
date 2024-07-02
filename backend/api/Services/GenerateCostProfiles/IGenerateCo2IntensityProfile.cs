using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2IntensityProfile
    {
        Task<Co2IntensityDto> Generate(Guid caseId);
    }
}
