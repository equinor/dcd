using System.Linq;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class WellProjectService
    {
        private readonly DcdDbContext _context;

        public WellProjectService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WellProject> GetAll()
        {
            if (_context.WellProjects != null)
            {
                return _context.WellProjects
                         .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues);
            }
            else
            {
                return new List<WellProject>();
            }
        }
        public IEnumerable<WellProject> GetWellProjects(Guid projectId)
        {
            if (_context.WellProjects != null)
            {
                return _context.WellProjects
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<WellProject>();
            }
        }
        public WellProject GetWellProject(Guid wellProjectId)
        {
            if (_context.WellProjects != null)
            {
                var wellProject = _context.WellProjects
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .FirstOrDefault(p => p.Id.Equals(wellProjectId));
                if (wellProject == null)
                {
                    throw new NotFoundInDBException(string.Format("Well Project %s not found", wellProject));
                }
                return wellProject;
            }
            throw new NotFoundInDBException($"The database contains no well projects");
        }
    }
}

