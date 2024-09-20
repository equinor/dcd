using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITopsideService
{
    Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto);
    Task<Topside> GetTopside(Guid topsideId);
    Task<TopsideDto> UpdateTopside<TDto>(Guid projectId, Guid caseId, Guid topsideId, TDto updatedTopsideDto) where TDto : BaseUpdateTopsideDto;
    Task<Topside> GetTopsideWithIncludes(Guid topsideId, params Expression<Func<Topside, object>>[] includes);
}
