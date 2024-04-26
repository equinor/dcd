using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<SurfDto> CopySurf(Guid surfId, Guid sourceCaseId);
    Task<ProjectDto> UpdateSurf<TDto>(TDto updatedSurfDto, Guid surfId) where TDto : BaseUpdateSurfDto;
    Task<Surf> GetSurf(Guid surfId);
    Task<ProjectDto> CreateSurf(SurfDto surfDto, Guid sourceCaseId);
    Task<Surf> NewCreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto);
}
