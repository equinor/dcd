using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ISubstructureService
    {
        SubstructureDto CopySubstructure(Guid substructureId, Guid sourceCaseId);
        ProjectDto CreateSubstructure(Substructure substructure, Guid sourceCaseId);
        ProjectDto DeleteSubstructure(Guid substructureId);
        Substructure GetSubstructure(Guid substructureId);
        Substructure NewCreateSubstructure(SubstructureDto substructureDto, Guid sourceCaseId);
        SubstructureDto NewUpdateSubstructure(SubstructureDto updatedSubstructureDto);
        ProjectDto UpdateSubstructure(SubstructureDto updatedSubstructureDto);
    }
}
