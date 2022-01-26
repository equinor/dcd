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
    }
}
