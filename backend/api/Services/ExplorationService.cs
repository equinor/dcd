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
        public Exploration GetExploration(Guid wellProjectId)
        {
            if (_context.Explorations != null)
            {
                var exploration = _context.Explorations
                        .Include(c => c.GAndGAdminCost)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .FirstOrDefault(p => p.Id.Equals(wellProjectId));
                if (exploration == null)
                {
                    throw new NotFoundInDBException(string.Format("Exploration %s not found", exploration));
                }
                return exploration;
            }
            throw new NotFoundInDBException($"The database contains no explorations");
        }
    }
}

