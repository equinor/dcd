using api.Models.Interfaces;

namespace api.Features.ProjectAccess;

public interface IProjectAccessService
{
    Task ProjectExists<T>(Guid projectIdFromUrl, Guid entityId) where T : class, IHasProjectId;
    Task<AccessRightsDto> GetUserProjectAccess(Guid externalId);
}
