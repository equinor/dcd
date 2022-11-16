using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly ILogger<TransportService> _logger;

    public TransportService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TransportService>();
    }
    public ProjectDto CreateTransport(TransportDto transportDto, Guid sourceCaseId)
    {
        var transport = TransportAdapter.Convert(transportDto);
        var project = _projectService.GetProjectWithAssets(transport.ProjectId);
        transport.Project = project;
        transport.ProspVersion = transportDto.ProspVersion;
        transport.LastChangedDate = DateTimeOffset.Now;
        transport.GasExportPipelineLength = transportDto.GasExportPipelineLength;
        transport.OilExportPipelineLength = transportDto.OilExportPipelineLength;
        _context.Transports!.Add(transport);
        _context.SaveChanges();
        SetCaseLink(transport, sourceCaseId, project);
        return _projectService.GetProjectDto(transport.ProjectId);
    }

    public Transport NewCreateTransport(TransportDto transportDto, Guid sourceCaseId)
    {
        var transport = TransportAdapter.Convert(transportDto);
        var project = _projectService.GetProjectWithAssets(transport.ProjectId);
        transport.Project = project;
        transport.LastChangedDate = DateTimeOffset.Now;
        var createdTransport = _context.Transports!.Add(transport);
        _context.SaveChanges();
        SetCaseLink(transport, sourceCaseId, project);
        return createdTransport.Entity;
    }

    public TransportDto CopyTransport(Guid transportId, Guid sourceCaseId)
    {
        var source = GetTransport(transportId);
        var newTransportDto = TransportDtoAdapter.Convert(source);
        newTransportDto.Id = Guid.Empty;
        if (newTransportDto.CostProfile != null)
        {
            newTransportDto.CostProfile.Id = Guid.Empty;
        }
        if (newTransportDto.CessationCostProfile != null)
        {
            newTransportDto.CessationCostProfile.Id = Guid.Empty;
        }

        var topside = NewCreateTransport(newTransportDto, sourceCaseId);
        var dto = TransportDtoAdapter.Convert(topside);

        return dto;
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
            .Include(c => c.CessationCostProfile)
            .FirstOrDefault(c => c.Id == transportId);
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
                .Include(c => c.CessationCostProfile)
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

        if (updatedTransportDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.TransportCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        _context.Transports!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedTransportDto.ProjectId);
    }

    public TransportDto NewUpdateTransport(TransportDto updatedTransportDto)
    {
        var existing = GetTransport(updatedTransportDto.Id);
        TransportAdapter.ConvertExisting(existing, updatedTransportDto);

        if (updatedTransportDto.CostProfile == null && existing.CostProfile != null)
        {
            _context.TransportCostProfile!.Remove(existing.CostProfile);
        }

        if (updatedTransportDto.CessationCostProfile == null && existing.CessationCostProfile != null)
        {
            _context.TransportCessationCostProfiles!.Remove(existing.CessationCostProfile);
        }

        existing.LastChangedDate = DateTimeOffset.Now;
        var updatedTransport = _context.Transports!.Update(existing);
        _context.SaveChanges();
        return TransportDtoAdapter.Convert(updatedTransport.Entity);
    }
}
