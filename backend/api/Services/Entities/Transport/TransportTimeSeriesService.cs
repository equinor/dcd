using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TransportTimeSeriesService : ITransportTimeSeriesService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TransportService> _logger;
    private readonly IMapper _mapper;
    private readonly ICaseRepository _caseRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly ITransportTimeSeriesRepository _repository;
    private readonly IMapperService _mapperService;

    public TransportTimeSeriesService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ICaseRepository caseRepository,
        ITransportRepository transportRepository,
        ITransportTimeSeriesRepository repository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TransportService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = repository;
        _transportRepository = transportRepository;
        _mapperService = mapperService;
    }

    public async Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto
    )
    {
        var transport = await _transportRepository.GetTransport(transportId)
            ?? throw new NotFoundInDBException($"Transport with id {transportId} not found.");

        var resourceHasProfile = await _transportRepository.TransportHasCostProfileOverride(transportId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Transport with id {transportId} already has a profile of type {typeof(TransportCostProfileOverride).Name}.");
        }

        TransportCostProfileOverride profile = new()
        {
            Transport = transport,
        };

        var newProfile = _mapperService.MapToEntity(dto, profile, transportId);

        TransportCostProfileOverride createdProfile;
        try
        {
            createdProfile = _repository.CreateTransportCostProfileOverride(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile TransportCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
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
        var transport = await _transportRepository.GetTransportWithCostProfile(transportId)
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
