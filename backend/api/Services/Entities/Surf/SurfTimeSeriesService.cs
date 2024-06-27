using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfTimeSeriesService : ISurfTimeSeriesService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    private readonly IMapper _mapper;
    private readonly ISurfRepository _surfRepository;
    private readonly ISurfTimeSeriesRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;
    public SurfTimeSeriesService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ISurfTimeSeriesRepository repository,
        ISurfRepository surfRepository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
        _mapper = mapper;
        _repository = repository;
        _surfRepository = surfRepository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    )
    {
        var surf = await _surfRepository.GetSurf(surfId)
            ?? throw new NotFoundInDBException($"Surf with id {surfId} not found.");

        var resourceHasProfile = await _surfRepository.SurfHasCostProfileOverride(surfId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Surf with id {surfId} already has a profile of type {typeof(SurfCostProfileOverride).Name}.");
        }

        SurfCostProfileOverride profile = new()
        {
            Surf = surf,
        };

        var newProfile = _mapperService.MapToEntity(dto, profile, surfId);

        SurfCostProfileOverride createdProfile;
        try
        {
            createdProfile = _repository.CreateSurfCostProfileOverride(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile SurfCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    )
    {
        var surf = await _surfRepository.GetSurfWithCostProfile(surfId)
            ?? throw new NotFoundInDBException($"Surf with id {surfId} not found.");

        if (surf.CostProfile != null)
        {
            return await UpdateSurfCostProfile(caseId, surfId, surf.CostProfile.Id, dto);
        }

        return await CreateSurfCostProfile(caseId, surfId, dto, surf);
    }

    private async Task<SurfCostProfileDto> UpdateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        Guid profileId,
        UpdateSurfCostProfileDto dto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfile, SurfCostProfileDto, UpdateSurfCostProfileDto>(
            caseId,
            surfId,
            profileId,
            dto,
            _repository.GetSurfCostProfile,
            _repository.UpdateSurfCostProfile
        );
    }

    private async Task<SurfCostProfileDto> CreateSurfCostProfile(
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto,
        Surf surf
    )
    {
        SurfCostProfile surfCostProfile = new SurfCostProfile
        {
            Surf = surf
        };

        var newProfile = _mapperService.MapToEntity(dto, surfCostProfile, surfId);

        try
        {
            _repository.CreateSurfCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    )
    {
        return await UpdateSurfTimeSeries<SurfCostProfileOverride, SurfCostProfileOverrideDto, UpdateSurfCostProfileOverrideDto>(
            caseId,
            surfId,
            costProfileId,
            updatedSurfCostProfileOverrideDto,
            _repository.GetSurfCostProfileOverride,
            _repository.UpdateSurfCostProfileOverride
        );
    }

    private async Task<TDto> UpdateSurfTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid surfId,
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

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, surfId);

        // TProfile updatedProfile;
        try
        {
            // updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(existingProfile, profileId);
        return updatedDto;
    }
}
