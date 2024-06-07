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

        // var transport = await NewCreateTransport(newTransportDto, sourceCaseId);
        // var dto = TransportDtoAdapter.Convert(transport);

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
            updatedTransport = _repository.UpdateTransport(existing);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Failed to update transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }


        var dto = _mapperService.MapToDto<Transport, TransportDto>(updatedTransport, transportId);
        return dto;
    }

    public async Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto)
    {
        return await UpdateTransportTimeSeries<TransportCostProfileOverride, TransportCostProfileOverrideDto, UpdateTransportCostProfileOverrideDto>(
            caseId,
            transportId,
            costProfileId,
            dto,
            _repository.GetTransportCostProfileOverride,
            _repository.UpdateTransportCostProfileOverride
        );
    }

    public async Task<TransportCostProfileDto> AddOrUpdateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto
    )
    {
        var transport = await _repository.GetTransportWithCostProfile(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        if (transport.CostProfile != null)
        {
            return await UpdateTransportCostProfile(caseId, transportId, transport.CostProfile.Id, dto);
        }

        return await CreateTransportCostProfile(caseId, transportId, dto, transport);
    }

    private async Task<TransportCostProfileDto> UpdateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        Guid profileId,
        UpdateTransportCostProfileDto dto
    )
    {
        return await UpdateTransportTimeSeries<TransportCostProfile, TransportCostProfileDto, UpdateTransportCostProfileDto>(
            caseId,
            transportId,
            profileId,
            dto,
            _repository.GetTransportCostProfile,
            _repository.UpdateTransportCostProfile
        );
    }

    private async Task<TransportCostProfileDto> CreateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto,
        Transport transport
    )
    {
        TransportCostProfile transportCostProfile = new TransportCostProfile
        {
            Transport = transport
        };

        var newProfile = _mapperService.MapToEntity(dto, transportCostProfile, transportId);

        try
        {
            _repository.CreateTransportCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for transport with id {transportId} for case id {caseId}.", transportId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

        private async Task<TDto> UpdateTransportTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid transportId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, transportId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, profileId);
        return updatedDto;
    }
}
