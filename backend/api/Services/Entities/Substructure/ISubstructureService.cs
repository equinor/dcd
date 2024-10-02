using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISubstructureService
{
    Task<Substructure> GetSubstructureWithIncludes(Guid substructureId, params Expression<Func<Substructure, object>>[] includes);
    Task<SubstructureDto> UpdateSubstructure<TDto>(Guid projectId, Guid caseId, Guid substructureId, TDto updatedSubstructureDto) where TDto : BaseUpdateSubstructureDto;
}
