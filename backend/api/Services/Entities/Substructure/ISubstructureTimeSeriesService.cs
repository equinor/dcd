using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureTimeSeriesService
{
    Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    );
    Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid substructureId,
        CreateSubstructureCostProfileOverrideDto dto
    );
    Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(Guid projectId, Guid caseId, Guid substructureId, Guid costProfileId, UpdateSubstructureCostProfileOverrideDto dto);
}
