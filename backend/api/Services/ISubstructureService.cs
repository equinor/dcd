using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureService
{
    Task<Substructure> CreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto);
    Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId);
    Task<SubstructureDto> UpdateSubstructure<TDto>(TDto updatedSubstructureDto, Guid substructureId) where TDto : BaseUpdateSubstructureDto;
    Task<Substructure> GetSubstructure(Guid substructureId);
}
