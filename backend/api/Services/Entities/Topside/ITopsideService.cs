using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideService
{
    Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto);
    Task<Topside> GetTopside(Guid topsideId);
    Task<TopsideDto> UpdateTopside<TDto>(Guid caseId, Guid topsideId, TDto updatedTopsideDto) where TDto : BaseUpdateTopsideDto;
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