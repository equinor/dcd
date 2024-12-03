using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Create;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public interface ITopsideTimeSeriesService
{
    Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    );
    Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto
    );
    Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(Guid projectId, Guid caseId, Guid topsideId, Guid costProfileId, UpdateTopsideCostProfileOverrideDto dto);
}
