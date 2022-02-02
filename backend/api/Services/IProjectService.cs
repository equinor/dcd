using api.Models;

namespace api.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAll();
        Project GetProject(Guid projectId);
        void AddSurfsToProject(Project project, Surf surf);
        void AddDrainageStrategy(Project project, DrainageStrategy drainageStrategy);
        void AddSubstructure(Project project, Substructure substructure);
    }
}
