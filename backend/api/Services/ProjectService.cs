using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ProjectService
    {
        private readonly DcdDbContext _context;

        public ProjectService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            if (_context.Projects != null)
            {
                return _context.Projects
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.DrainageStrategy)
                            .ThenInclude(c => c.ProductionProfileOil)
                                .ThenInclude(c => c.YearValues)
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.DrainageStrategy)
                            .ThenInclude(c => c.ProductionProfileGas)
                                .ThenInclude(c => c.YearValues);
            }
            else
            {
                return new List<Project>();
            }
        }

        public Project GetProject(Guid projectId)
        {
            if (_context.Projects != null)
            {
                var project = _context.Projects
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.DrainageStrategy)
                            .ThenInclude(c => c.ProductionProfileOil)
                    .Include(c => c.Cases)
                        .ThenInclude(c => c.DrainageStrategy)
                            .ThenInclude(c => c.ProductionProfileGas)
                              .FirstOrDefault(p => p.Id.Equals(projectId));
                if (project == null)
                {
                    throw new NotFoundInDBException(string.Format("Project %s not found", projectId));
                }
                return project;
            }
            throw new NotFoundInDBException($"The database contains no projects");
        }
    }
}
