using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfTimeSeriesService
{
    Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    );
    Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    );

    Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    );
}
