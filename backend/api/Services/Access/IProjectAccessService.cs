using api.Models.Interfaces;

namespace api.Services
{
    public interface IProjectAccessService
    {
        Task ProjectExists<T>(Guid projectIdFromUrl, Guid entityId)
        where T : class, IHasProjectId;
    }
}
