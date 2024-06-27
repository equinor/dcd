using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IFuelFlaringLossesProfileService
    {
        Task<FuelFlaringAndLossesDto> Generate(Guid caseId);
    }
}
