using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ISubstructureService
    {
        Task<ProjectDto> CreateSubstructure(Substructure substructure, Guid sourceCaseId);
        Task<Substructure> NewCreateSubstructure(SubstructureDto substructureDto, Guid sourceCaseId);
        Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId);
        Task<ProjectDto> DeleteSubstructure(Guid substructureId);
        Task<ProjectDto> UpdateSubstructure(SubstructureDto updatedSubstructureDto);
        Task<Substructure> GetSubstructure(Guid substructureId);
    }
}
