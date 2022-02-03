using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TransportService : ITransportService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public TransportService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }
        public Project CreateTransport(Transport transport)
        {
            if (_context.Transports != null)
            {
                var result = _context.Transports.Add(transport);
                _context.SaveChanges();
                return _projectService.GetProject(result.Entity.ProjectId);
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }


        public Project DeleteTransport(Transport transport)
        {
            if (_context.Transports != null)
            {
                var transportFromDb = GetTransport(transport.Id);
                _context.Transports.Remove(transportFromDb);
                _context.SaveChanges();
                return _projectService.GetProject(transportFromDb.ProjectId);
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }


        public Transport GetTransport(Guid transportId)
        {
            if (_context.Transports != null)
            {
                return _context.Transports
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Id.Equals(transportId)).First();
            }
            else
            {
                return new Transport();
            }
        }

        public IEnumerable<Transport> GetTransports(Guid projectId)
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


        public Project UpdateTransport(Guid OldTransportId, Transport changedtransport)
        {
            if (_context.Transports != null)
            {
                var transportFromDb = GetTransport(OldTransportId);
                var updatedTransport = CopyData(transportFromDb, changedtransport);
                _context.Transports.Update(updatedTransport);
                _context.SaveChanges();
                return _projectService.GetProject(transportFromDb.ProjectId);
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }

        private Transport CopyData(Transport transport, Transport updatedTransport)
        {
            transport.Name = updatedTransport.Name;
            transport.GasExportPipelineLength = updatedTransport.GasExportPipelineLength;
            transport.OilExportPipelineLength = updatedTransport.OilExportPipelineLength;
            transport.Maturity = updatedTransport.Maturity;
            transport.Project = updatedTransport.Project;
            transport.ProjectId = updatedTransport.ProjectId;
            transport.CostProfile = updatedTransport.CostProfile;
            return transport;
        }

    }
}
