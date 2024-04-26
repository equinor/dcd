using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideService
{
    Task<TopsideDto> CopyTopside(Guid topsideId, Guid sourceCaseId);
    Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto);
    Task<ProjectDto> UpdateTopside<TDto>(TDto updatedTopsideDto, Guid topsideId) where TDto : BaseUpdateTopsideDto;
    Task<Topside> GetTopside(Guid topsideId);
}
