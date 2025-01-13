using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Substructures.Services;

public interface ISubstructureService
{
    Task<SubstructureDto> UpdateSubstructure<TDto>(Guid projectId, Guid caseId, Guid substructureId, TDto updatedSubstructureDto) where TDto : BaseUpdateSubstructureDto;
}
