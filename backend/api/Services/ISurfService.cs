using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<SurfDto> CopySurf(Guid surfId, Guid sourceCaseId);
    Task<ProjectDto> UpdateSurf(SurfDto updatedSurfDto);
    Task<Surf> GetSurf(Guid surfId);
    Task<ProjectDto> CreateSurf(SurfDto surfDto, Guid sourceCaseId);
    Task<Surf> NewCreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto);
}
