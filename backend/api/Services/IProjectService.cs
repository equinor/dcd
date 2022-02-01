using api.Models;

namespace api.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAll();
        Project GetProject(Guid projectId);
    }
}
