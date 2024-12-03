using api.Features.CaseProfiles.Dtos;

namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface ICo2DrillingFlaringFuelTotalsService
{
    Task<Co2DrillingFlaringFuelTotalsDto> Generate(Guid caseId);
}
