using api.Adapters;
using api.Context;
using api.Dtos;
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
        public ProjectDto CreateTransport(TransportDto transportDto, Guid sourceCaseId)
        {
            var transport = TransportAdapter.Convert(transportDto);
            var project = _projectService.GetProject(transport.ProjectId);
            transport.Project = project;
            _context.Transports!.Add(transport);
            _context.SaveChanges();
            SetCaseLink(transport, sourceCaseId, project);
            return _projectService.GetProjectDto(transport.ProjectId);
        }

        private void SetCaseLink(Transport transport, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases?.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.TransportLink = transport.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteTransport(Guid transportId)
        {
            var transport = GetTransport(transportId);
            _context.Transports!.Remove(transport);
            DeleteCaseLinks(transportId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(transport.ProjectId);
        }

        public Transport GetTransport(Guid transportId)
        {
            var transport = _context.Transports!
                    .Include(c => c.CostProfile)
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
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Transport>();
            }
        }

        public ProjectDto UpdateTransport(TransportDto updatedTransportDto)
        {
            var existing = GetTransport(updatedTransportDto.Id);
            TransportAdapter.ConvertExisting(existing, updatedTransportDto);

            if (updatedTransportDto.CostProfile == null && existing.CostProfile != null)
            {
                _context.TransportCostProfile!.Remove(existing.CostProfile);
            }

            _context.Transports!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }
    }
}
