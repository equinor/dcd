using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SubstructureService
    {
        private readonly DcdDbContext _context;

        public SubstructureService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Substructure> GetSubstructuresForProject(Guid projectId)
        {
            if (_context.Substructures != null)
            {
                return _context.Substructures
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Substructure>();
            }
        }
    }
}
