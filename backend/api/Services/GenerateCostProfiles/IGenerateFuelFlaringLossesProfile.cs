using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateFuelFlaringLossesProfile
    {
        FuelFlaringAndLossesDto Generate(Guid caseId);
    }
}
