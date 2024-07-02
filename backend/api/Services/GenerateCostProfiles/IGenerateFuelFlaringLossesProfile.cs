using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateFuelFlaringLossesProfile
    {
        Task<FuelFlaringAndLossesDto> Generate(Guid caseId);
    }
}
