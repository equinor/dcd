using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideService
{
    Task<TopsideDto> CopyTopside(Guid topsideId, Guid sourceCaseId);
    Task<ProjectDto> CreateTopside(TopsideDto topsideDto, Guid sourceCaseId);
    Task<Topside> NewCreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto);
    Task<ProjectDto> UpdateTopside(TopsideDto updatedTopsideDto);
    Task<Topside> GetTopside(Guid topsideId);
}
