using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideTimeSeriesService
{
    Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    );
    Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto
    );
    Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(Guid caseId, Guid topsideId, Guid costProfileId, UpdateTopsideCostProfileOverrideDto dto);
}
