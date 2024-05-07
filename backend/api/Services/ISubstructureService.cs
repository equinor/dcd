using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureService
{
    Task<Substructure> CreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto);
    Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId);
    Task<SubstructureDto> UpdateSubstructureAndCostProfiles<TDto>(TDto updatedSubstructureDto, Guid substructureId) where TDto : BaseUpdateSubstructureDto;
    Task<Substructure> GetSubstructure(Guid substructureId);
    Task<SubstructureDto> UpdateSubstructure<TDto>(Guid caseId, Guid substructureId, TDto updatedSubstructureDto) where TDto : BaseUpdateSubstructureDto;
    Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(Guid caseId, Guid substructureId, Guid costProfileId, UpdateSubstructureCostProfileOverrideDto dto);
}
