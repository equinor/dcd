using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Create;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

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
