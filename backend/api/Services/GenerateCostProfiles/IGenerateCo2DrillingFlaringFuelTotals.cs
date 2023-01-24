using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2DrillingFlaringFuelTotals
    {
        Co2DrillingFlaringFuelTotalsDto Generate(Guid caseId);
    }
}
