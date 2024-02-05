using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2DrillingFlaringFuelTotals
    {
        Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid caseId);
    }
}
