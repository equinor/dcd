using api.Context;
using api.Models;

namespace api.Services
{
    public class ProjectService
    {
        private readonly DcdDbContext _context;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
        }

        public IQueryable<Project>? GetAll()
        {
            return _context.Projects;
        }

        public Project GetProject(string projectId)
        {
            if (_context.Projects != null)
            {
                var project = _context.Projects.FirstOrDefault(p => p.Id.Equals(projectId));
                if (project == null)
                {
                    throw new NotFoundInDBException($"Project not found: {projectId}");
                }
                return project;
            }
            throw new NotFoundInDBException($"The database contains no projects");
        }
    }
}
