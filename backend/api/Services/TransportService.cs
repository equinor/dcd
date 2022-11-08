using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService
{
    private readonly DcdDbContext _context;
    private readonly ILogger<TransportService> _logger;
    private readonly ProjectService _projectService;

    public TransportService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TransportService>();
    }

    public async Task<ProjectDto> CreateTransport(TransportDto transportDto, Guid sourceCaseId)
    {
        var transport = TransportAdapter.Convert(transportDto);
        var project = _projectService.GetProject(transport.ProjectId);
        transport.Project = project;
        transport.ProspVersion = transportDto.ProspVersion;
        transport.LastChangedDate = DateTimeOffset.Now;
        transport.GasExportPipelineLength = transportDto.GasExportPipelineLength;
        transport.OilExportPipelineLength = transportDto.OilExportPipelineLength;
        _context.Transports!.Add(transport);
        await _context.SaveChangesAsync();
        SetCaseLink(transport, sourceCaseId, project);
        return _projectService.GetProjectDto(transport.ProjectId);
    }

    public async Task<Transport> NewCreateTransport(TransportDto transportDto, Guid sourceCaseId)
    {
        var transport = TransportAdapter.Convert(transportDto);
        var project = _projectService.GetProject(transport.ProjectId);
        transport.Project = project;
        transport.LastChangedDate = DateTimeOffset.Now;
        var createdTransport = _context.Transports!.Add(transport);
        await _context.SaveChangesAsync();
        SetCaseLink(transport, sourceCaseId, project);
        return createdTransport.Entity;
    }

    private void SetCaseLink(Transport transport, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases?.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException($"Case {sourceCaseId} not found in database.");
        }

        case_.TransportLink = transport.Id;
        _context.SaveChanges();
    }

    public async Task<ProjectDto> DeleteTransport(Guid transportId)
    {
        var transport = await GetTransport(transportId);
        _context.Transports!.Remove(transport);
        DeleteCaseLinks(transportId);
        await _context.SaveChangesAsync();
        return _projectService.GetProjectDto(transport.ProjectId);
    }

    public async Task<Transport> GetTransport(Guid transportId)
    {
        var transport = await _context.Transports!
            .Include(c => c.CostProfile)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(c => c.Id == transportId);
        if (transport == null)
        {
            throw new ArgumentException($"Transport {transportId} not found.");
        }

        return transport;
    }

    private void DeleteCaseLinks(Guid transportId)
    {
        foreach (var c in _context.Cases!)
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

        return new List<Transport>();
    }

    public ProjectDto UpdateTransport(TransportDto updatedTransportDto)
    {
        var existing = GetTransport(updatedTransportDto.Id).Result;
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
        var existing = GetTransport(updatedTransportDto.Id).Result;
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
