using api.Models.Interfaces;

namespace api.Features.ProjectIntegrity;

public interface IProjectIntegrityService
{
    Task EntityIsConnectedToProject<T>(Guid projectIdFromUrl, Guid entityId) where T : class, IHasProjectId;
}
