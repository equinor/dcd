using System;
using System.Linq;

using api.Context;
using api.Models;

namespace api.Services
{
    public class ProjectService
    {
        private readonly DCDDbContext _context;

        public ProjectService(DCDDbContext context)
        {
            _context = context;
        }

        public IQueryable<Project> GetAll()
        {
            return _context.Projects;
        }

        public Project GetProject(string projectId)
        {
            Project project = _context.Projects.First(project => project.Id.Equals(projectId));
            if (project == null)
            {
                throw new Exception($"Project not found: {projectId}");
            }
            return project;
        }
    }
}
