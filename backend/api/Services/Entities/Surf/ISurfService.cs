using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface ISurfService
{
    Task<Surf> GetSurfWithIncludes(Guid surfId, params Expression<Func<Surf, object>>[] includes);
    Task<SurfDto> UpdateSurf<TDto>(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    ) where TDto : BaseUpdateSurfDto;
}
