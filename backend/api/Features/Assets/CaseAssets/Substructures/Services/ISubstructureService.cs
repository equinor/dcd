using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

public interface ISubstructureService
{
    Task<Substructure> GetSubstructureWithIncludes(Guid substructureId, params Expression<Func<Substructure, object>>[] includes);
    Task<SubstructureDto> UpdateSubstructure<TDto>(Guid projectId, Guid caseId, Guid substructureId, TDto updatedSubstructureDto) where TDto : BaseUpdateSubstructureDto;
}
