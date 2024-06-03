using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportService : ITransportService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TransportService> _logger;
    private readonly IMapper _mapper;
    private readonly ICaseRepository _caseRepository;
    private readonly ITransportRepository _repository;
    private readonly IMapperService _mapperService;

    public TransportService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ICaseRepository caseRepository,
        ITransportRepository transportRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TransportService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = transportRepository;
        _mapperService = mapperService;
    }

    public async Task<Transport> CreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto)
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

    public async Task<TransportWithProfilesDto> CopyTransport(Guid transportId, Guid sourceCaseId)
    {
        var source = await GetTransport(transportId);
        var newTransportDto = _mapper.Map<TransportWithProfilesDto>(source);
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

    public async Task<ProjectDto> UpdateTransportAndCostProfiles<TDto>(TDto updatedTransportDto, Guid transportId)
        where TDto : BaseUpdateTransportDto
    {
        var existing = await GetTransport(transportId);

        _mapper.Map(updatedTransportDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Transports!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
    }

    public async Task<TransportDto> UpdateTransport<TDto>(Guid caseId, Guid transportId, TDto updatedTransportDto)
        where TDto : BaseUpdateTransportDto
    {
        var existing = await _repository.GetTransport(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        _mapperService.MapToEntity(updatedTransportDto, existing, transportId);
        existing.LastChangedDate = DateTimeOffset.UtcNow;

        Transport updatedTransport;
        try
        {
            updatedTransport = await _repository.UpdateTransport(existing);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Failed to update transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var dto = _mapperService.MapToDto<Transport, TransportDto>(updatedTransport, transportId);
        return dto;
    }

    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto)
    {
        var existingCostProfile = await _repository.GetTransportCostProfileOverride(costProfileId)
            ?? throw new NotFoundInDBException($"Transport cost profile override with id {costProfileId} not found.");

        _mapperService.MapToEntity(dto, existingCostProfile, costProfileId);

        TransportCostProfileOverride updatedCostProfile;
        try
        {
            updatedCostProfile = await _repository.UpdateTransportCostProfileOverride(existingCostProfile);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Failed to update transport cost profile override with id {costProfileId} for transport id {transportId}.", costProfileId, transportId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var updatedDto = _mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(updatedCostProfile, costProfileId);

        return updatedDto;
    }
}
