using System.Linq;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ExplorationService
    {
        private readonly DcdDbContext _context;

        public ExplorationService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Exploration> GetExplorations(Guid projectId)
        {
            if (_context.Explorations != null)
            {
                return _context.Explorations
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GAndGAdminCost)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Exploration>();
            }
        }
    }
}

