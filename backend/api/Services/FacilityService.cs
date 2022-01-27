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

        public IEnumerable<Surf> GetSurfsForProject(Guid projectId)
        {
            if (_context.Surfs != null)
            {
                return _context.Surfs
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.InfieldPipelineSystemLength)
                        .Include(c => c.UmbilicalSystemLength)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Surf>();
            }
        }

        public IEnumerable<Substructure> GetSubstructuresForProject(Guid projectId)
        {
            if (_context.Substructures != null)
            {
                return _context.Substructures
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DryWeight)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Substructure>();
            }
        }

        public IEnumerable<Topside> GetTopsidesForProject(Guid projectId)
        {
            if (_context.Topsides != null)
            {
                return _context.Topsides
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DryWeight)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }

        public IEnumerable<Transport> GetTransportsForProject(Guid projectId)
        {
            if (_context.Transports != null)
            {
                return _context.Transports
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GasExportPipelineLength)
                        .Include(c => c.OilExportPipelineLength)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Transport>();
            }
        }
    }
}
