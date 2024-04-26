using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureService
{
    Task<ProjectDto> CreateSubstructure(Substructure substructure, Guid sourceCaseId);
    Task<Substructure> NewCreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto);
    Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId);
    Task<ProjectDto> UpdateSubstructure<TDto>(TDto updatedSubstructureDto, Guid substructureId) where TDto : BaseUpdateSubstructureDto;
    Task<Substructure> GetSubstructure(Guid substructureId);
}
