using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideTimeSeriesService : ITopsideTimeSeriesService
{
    private readonly ILogger<TopsideService> _logger;
    private readonly IProjectAccessService _projectAccessService;
    private readonly ITopsideTimeSeriesRepository _repository;
    private readonly ITopsideRepository _topsideRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public TopsideTimeSeriesService(
        ILoggerFactory loggerFactory,
        ITopsideTimeSeriesRepository repository,
        ITopsideRepository topsideRepository,
        ICaseRepository caseRepository,
        IMapperService mapperService,
        IProjectAccessService projectAccessService
        )
    {
        _logger = loggerFactory.CreateLogger<TopsideService>();
        _repository = repository;
        _topsideRepository = topsideRepository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
        _projectAccessService = projectAccessService;
    }

    public async Task<TopsideCostProfileOverrideDto> CreateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        CreateTopsideCostProfileOverrideDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var topside = await _topsideRepository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        var resourceHasProfile = await _topsideRepository.TopsideHasCostProfileOverride(topsideId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Topside with id {topsideId} already has a profile of type {typeof(TopsideCostProfileOverride).Name}.");
        }

        TopsideCostProfileOverride profile = new()
        {
            Topside = topside,
        };

        var newProfile = _mapperService.MapToEntity(dto, profile, topsideId);

        TopsideCostProfileOverride createdProfile;
        try
        {
            createdProfile = _repository.CreateTopsideCostProfileOverride(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile TopsideCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid costProfileId,
        UpdateTopsideCostProfileOverrideDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfileOverride, TopsideCostProfileOverrideDto, UpdateTopsideCostProfileOverrideDto>(
            projectId,
            caseId,
            topsideId,
            costProfileId,
            dto,
            _repository.GetTopsideCostProfileOverride,
            _repository.UpdateTopsideCostProfileOverride
        );
    }

    public async Task<TopsideCostProfileDto> AddOrUpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto
    )
    {
        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Topside>(projectId, topsideId);

        var topside = await _topsideRepository.GetTopsideWithCostProfile(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        if (topside.CostProfile != null)
        {
            return await UpdateTopsideCostProfile(projectId, caseId, topsideId, topside.CostProfile.Id, dto);
        }

        return await CreateTopsideCostProfile(caseId, topsideId, dto, topside);
    }

    private async Task<TopsideCostProfileDto> UpdateTopsideCostProfile(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        UpdateTopsideCostProfileDto dto
    )
    {
        return await UpdateTopsideTimeSeries<TopsideCostProfile, TopsideCostProfileDto, UpdateTopsideCostProfileDto>(
            projectId,
            caseId,
            topsideId,
            profileId,
            dto,
            _repository.GetTopsideCostProfile,
            _repository.UpdateTopsideCostProfile
        );
    }

    private async Task<TopsideCostProfileDto> CreateTopsideCostProfile(
        Guid caseId,
        Guid topsideId,
        UpdateTopsideCostProfileDto dto,
        Topside topside
    )
    {
        TopsideCostProfile topsideCostProfile = new()
        {
            Topside = topside
        };

        var newProfile = _mapperService.MapToEntity(dto, topsideCostProfile, topsideId);
        if (newProfile.Topside.CostProfileOverride != null)
        {
            newProfile.Topside.CostProfileOverride.Override = false;
        }
        try
        {
            _repository.CreateTopsideCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for topside with id {topsideId} for case id {caseId}.", topsideId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    private async Task<TDto> UpdateTopsideTimeSeries<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid topsideId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, ITopsideTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await _projectAccessService.ProjectExists<Topside>(projectId, existingProfile.Topside.Id);

        if (existingProfile.Topside.ProspVersion == null)
        {
            if (existingProfile.Topside.CostProfileOverride != null)
            {
                existingProfile.Topside.CostProfileOverride.Override = true;
            }
        }

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, topsideId);

        try
        {
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
