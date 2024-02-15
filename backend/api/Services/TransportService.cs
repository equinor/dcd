using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService : ITransportService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TransportService> _logger;
    private readonly IMapper _mapper;

    public TransportService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TransportService>();
        _mapper = mapper;
    }
    public async Task<ProjectDto> CreateTransport(TransportDto transportDto, Guid sourceCaseId)
    {
        var transport = _mapper.Map<Transport>(transportDto);
        if (transport == null)
        {
            throw new ArgumentNullException(nameof(transport));
        }
        var project = await _projectService.GetProject(transport.ProjectId);
        transport.Project = project;
        transport.ProspVersion = transportDto.ProspVersion;
        transport.LastChangedDate = DateTimeOffset.UtcNow;
        transport.GasExportPipelineLength = transportDto.GasExportPipelineLength;
        transport.OilExportPipelineLength = transportDto.OilExportPipelineLength;
        _context.Transports!.Add(transport);
        await _context.SaveChangesAsync();
        await SetCaseLink(transport, sourceCaseId, project);
        return await _projectService.GetProjectDto(transport.ProjectId);
    }

    public async Task<Transport> NewCreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto)
    {
        var transport = _mapper.Map<Transport>(transportDto);
        if (transport == null)
        {
            throw new ArgumentNullException(nameof(transport));
        }
        var project = await _projectService.GetProject(projectId);
        transport.Project = project;
        transport.LastChangedDate = DateTimeOffset.UtcNow;
        var createdTransport = _context.Transports!.Add(transport);
        await _context.SaveChangesAsync();
        await SetCaseLink(transport, sourceCaseId, project);
        return createdTransport.Entity;
    }

    public async Task<TransportDto> CopyTransport(Guid transportId, Guid sourceCaseId)
    {
        var source = await GetTransport(transportId);
        var newTransportDto = _mapper.Map<TransportDto>(source);
        if (newTransportDto == null)
        {
            throw new ArgumentNullException(nameof(newTransportDto));
        }
        newTransportDto.Id = Guid.Empty;
        if (newTransportDto.CostProfile != null)
        {
            newTransportDto.CostProfile.Id = Guid.Empty;
        }
        if (newTransportDto.CostProfileOverride != null)
        {
            newTransportDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newTransportDto.CessationCostProfile != null)
        {
            newTransportDto.CessationCostProfile.Id = Guid.Empty;
        }

        // var topside = await NewCreateTransport(newTransportDto, sourceCaseId);
        // var dto = TransportDtoAdapter.Convert(topside);

        // return dto;
        return newTransportDto;
    }

    private async Task SetCaseLink(Transport transport, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.TransportLink = transport.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<Transport> GetTransport(Guid transportId)
    {
        var transport = await _context.Transports!
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(c => c.Id == transportId);
        if (transport == null)
        {
            throw new ArgumentException(string.Format("Transport {0} not found.", transportId));
        }
        return transport;
    }

    public async Task<ProjectDto> UpdateTransport(TransportDto updatedTransportDto)
    {
        var existing = await GetTransport(updatedTransportDto.Id);

        _mapper.Map(updatedTransportDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Transports!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(updatedTransportDto.ProjectId);
    }
}
