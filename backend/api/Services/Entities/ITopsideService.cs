using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideService
{
    Task<TopsideDto> CopyTopside(Guid topsideId, Guid sourceCaseId);
    Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto);
    Task<ProjectDto> UpdateTopsideAndCostProfiles<TDto>(TDto updatedTopsideDto, Guid topsideId) where TDto : BaseUpdateTopsideDto;
    Task<Topside> GetTopside(Guid topsideId);
    Task<TopsideDto> UpdateTopside<TDto>(Guid caseId, Guid topsideId, TDto updatedTopsideDto) where TDto : BaseUpdateTopsideDto;
    Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(Guid caseId, Guid topsideId, Guid costProfileId, UpdateTopsideCostProfileOverrideDto dto);
}
