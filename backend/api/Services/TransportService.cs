using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TransportService
    {
        private readonly DcdDbContext _context;

        public TransportService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Transport> GetTransportsForProject(Guid projectId)
        {
            if (_context.Transports != null)
            {
                return _context.Transports
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Transport>();
            }
        }
    }
}
