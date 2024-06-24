using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureTimeSeriesService
{
    Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    );
    Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        Guid caseId,
        Guid substructureId,
        CreateSubstructureCostProfileOverrideDto dto
    );
    Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(Guid caseId, Guid substructureId, Guid costProfileId, UpdateSubstructureCostProfileOverrideDto dto);
}
