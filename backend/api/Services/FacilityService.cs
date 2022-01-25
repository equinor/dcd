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

        public Surf GetSurf(Guid surfId)
        {
            if (_context.Surfs != null)
            {
                var surf = _context.Surfs
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.InfieldPipelineSystemLength)
                        .Include(c => c.UmbilicalSystemLength)
                    .FirstOrDefault(c => c.Id.Equals(surfId));
                if (surf == null)
                {
                    throw new NotFoundInDBException(string.Format("Surf {0} not found.", surfId));
                }
                return surf;
            }
            throw new NotFoundInDBException($"The database contains no surfs.");
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

        public Substructure GetSubstructure(Guid substructureId)
        {
            if (_context.Substructures != null)
            {
                var substructure = _context.Substructures
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DryWeight)
                    .FirstOrDefault(c => c.Id.Equals(substructureId));
                if (substructure == null)
                {
                    throw new NotFoundInDBException(string.Format("Substructure {0} not found.", substructureId));
                }
                return substructure;
            }
            throw new NotFoundInDBException($"The database contains no substructures.");
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

        public Topside GetTopside(Guid topsideId)
        {
            if (_context.Topsides != null)
            {
                var topside = _context.Topsides
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DryWeight)
                    .FirstOrDefault(c => c.Id.Equals(topsideId));
                if (topside == null)
                {
                    throw new NotFoundInDBException(string.Format("Topside {0} not found.", topsideId));
                }
                return topside;
            }
            throw new NotFoundInDBException($"The database contains no topsides.");
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

        public Transport GetTransport(Guid transportId)
        {
            if (_context.Transports != null)
            {
                var transport = _context.Transports
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GasExportPipelineLength)
                        .Include(c => c.OilExportPipelineLength)
                    .FirstOrDefault(c => c.Id.Equals(transportId));
                if (transport == null)
                {
                    throw new NotFoundInDBException(string.Format("Transport {0} not found.", transportId));
                }
                return transport;
            }
            throw new NotFoundInDBException($"The database contains no transports.");
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
