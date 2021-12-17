using System;
using System.Linq;

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
                Project project = _context.Projects.First(project => project.Id.Equals(projectId));
                if (project == null)
                {
                    throw new Exception($"Project not found: {projectId}");
                }
                return project;
            }
            throw new Exception($"Project not found: {projectId}");
        }
    }
}
