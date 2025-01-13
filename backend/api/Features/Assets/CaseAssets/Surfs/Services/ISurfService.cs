using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public interface ISurfService
{
    Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    ) where TDto : BaseUpdateSurfDto;
}
