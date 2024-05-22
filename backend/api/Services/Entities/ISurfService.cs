using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<SurfDto> CopySurf(Guid surfId, Guid sourceCaseId);
    Task<ProjectDto> UpdateSurfAndCostProfiles<TDto>(TDto updatedSurfDto, Guid surfId) where TDto : BaseUpdateSurfDto;
    Task<Surf> GetSurf(Guid surfId);
    Task<Surf> CreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto);
    Task<SurfDto> UpdateSurf<TDto>(
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    ) where TDto : BaseUpdateSurfDto;

    Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    );
}
