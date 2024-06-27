using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<Surf> GetSurf(Guid surfId);
    Task<Surf> CreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto);
    Task<SurfDto> UpdateSurf<TDto>(
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    ) where TDto : BaseUpdateSurfDto;
}
