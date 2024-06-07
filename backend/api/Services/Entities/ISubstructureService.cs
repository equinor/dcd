using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureService
{
    Task<Substructure> CreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto);
    Task<SubstructureWithProfilesDto> CopySubstructure(Guid substructureId, Guid sourceCaseId);
    Task<Substructure> GetSubstructure(Guid substructureId);
    Task<SubstructureDto> UpdateSubstructure<TDto>(Guid caseId, Guid substructureId, TDto updatedSubstructureDto) where TDto : BaseUpdateSubstructureDto;
    Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    );
    Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(Guid caseId, Guid substructureId, Guid costProfileId, UpdateSubstructureCostProfileOverrideDto dto);
}
