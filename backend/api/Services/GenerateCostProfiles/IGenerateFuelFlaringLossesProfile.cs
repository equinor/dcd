using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateFuelFlaringLossesProfile
    {
        Task<FuelFlaringAndLossesDto> GenerateAsync(Guid caseId);
    }
}
