using System.Linq;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FacilityService
    {
        private readonly DcdDbContext _context;

        public FacilityService(DcdDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Surf> GetAllSurfs()
        {
            if (_context.Surfs != null)
            {
                return _context.Surfs
                        .Include(c => c.CostProfile)
                        .Include(c => c.InfieldPipelineSystemLength)
                        .Include(c => c.UmbilicalSystemLength);
            }
            else
            {
                return new List<Surf>();
            }
        }

        public Surf GetSurf(Guid surfId)
        {
            if (_context.Surfs != null)
            {
                var surf = _context.Surfs
                        .Include(c => c.CostProfile)
                        .Include(c => c.InfieldPipelineSystemLength)
                        .Include(c => c.UmbilicalSystemLength)
                    .FirstOrDefault(p => p.Id.Equals(surfId));
                if (surf == null)
                {
                    throw new NotFoundInDBException(string.Format("Surf {0} not found", surfId));
                }
                return surf;
            }
            throw new NotFoundInDBException($"The database contains no surfs");
        }

        public IEnumerable<Surf> GetSurfsForProject(Guid projectId)
        {
            if (_context.Surfs != null)
            {
                return _context.Surfs
                        .Include(c => c.CostProfile)
                        .Include(c => c.InfieldPipelineSystemLength)
                        .Include(c => c.UmbilicalSystemLength)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Surf>();
            }
        }
    }
}
