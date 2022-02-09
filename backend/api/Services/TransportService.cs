using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TransportService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public TransportService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }
        public Project CreateTransport(Transport transport, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(transport.ProjectId);
            transport.Project = project;
            _context.Transports!.Add(transport);
            _context.SaveChanges();
            SetCaseLink(transport, sourceCaseId, project);
            return _projectService.GetProject(transport.ProjectId);
        }

        private void SetCaseLink(Transport transport, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.TransportLink = transport.Id;
            _context.SaveChanges();
        }

        public Project DeleteTransport(Guid transportId)
        {
            var transport = GetTransport(transportId);
            _context.Transports!.Remove(transport);
            DeleteCaseLinks(transportId);
            _context.SaveChanges();
            return _projectService.GetProject(transport.ProjectId);
        }

        public Transport GetTransport(Guid transportId)
        {
            var transport = _context.Transports!
                    .Include(c => c.CostProfile)
                        .ThenInclude(c => c.YearValues)
                .Where(c => c.Id == transportId).First();
            if (transport == null)
            {
                throw new ArgumentException(string.Format("Transport {0} not found.", transportId));
            }
            return transport;
        }

        private void DeleteCaseLinks(Guid transportId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.TransportLink == transportId)
                {
                    c.TransportLink = Guid.Empty;
                }
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

        public Project UpdateTransport(Guid transportId, Transport changedtransport)
        {
            var transport = GetTransport(transportId);
            CopyData(transport, changedtransport);
            _context.Transports!.Update(transport);
            _context.SaveChanges();
            return _projectService.GetProject(transport.ProjectId);
        }

        private static void CopyData(Transport transport, Transport updatedTransport)
        {
            transport.Name = updatedTransport.Name;
            transport.GasExportPipelineLength = updatedTransport.GasExportPipelineLength;
            transport.OilExportPipelineLength = updatedTransport.OilExportPipelineLength;
            transport.Maturity = updatedTransport.Maturity;
            transport.Project = updatedTransport.Project;
            transport.ProjectId = updatedTransport.ProjectId;
            transport.CostProfile = updatedTransport.CostProfile;
        }

    }
}
