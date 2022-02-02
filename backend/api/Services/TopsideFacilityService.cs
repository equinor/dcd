using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TopsideFacilityService
    {
        private readonly DcdDbContext _context;

        public TopsideFacilityService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Topside> GetTopsides(Guid projectId)
        {
            if (_context.Topsides != null)
            {
                return _context.Topsides
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }
    }
}
