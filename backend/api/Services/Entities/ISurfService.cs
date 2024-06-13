using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<SurfWithProfilesDto> CopySurf(Guid surfId, Guid sourceCaseId);
    Task<Surf> GetSurf(Guid surfId);
    Task<Surf> CreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto);
    Task<SurfDto> UpdateSurf<TDto>(
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    ) where TDto : BaseUpdateSurfDto;

    Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    );

    Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    );

    Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    );
}
