using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface ICo2DrillingFlaringFuelTotalsService
    {
        Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid caseId);
    }
}
