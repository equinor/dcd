using api.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

namespace api.Services.GenerateCostProfiles;

public interface ICo2IntensityProfileService
{
    Task<Co2IntensityDto> Generate(Guid caseId);
}
