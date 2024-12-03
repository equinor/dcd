using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public interface ITopsideService
{
    Task<TopsideDto> UpdateTopside<TDto>(Guid projectId, Guid caseId, Guid topsideId, TDto updatedTopsideDto) where TDto : BaseUpdateTopsideDto;
    Task<Topside> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes);
}
