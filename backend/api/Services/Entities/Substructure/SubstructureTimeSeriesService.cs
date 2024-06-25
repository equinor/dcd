
using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureTimeSeriesService : ISubstructureTimeSeriesService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SubstructureService> _logger;
    private readonly IMapper _mapper;
    private readonly ISubstructureRepository _substructureRepository;
    private readonly ISubstructureTimeSeriesRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public SubstructureTimeSeriesService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ISubstructureRepository substructureRepository,
        ISubstructureTimeSeriesRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SubstructureService>();
        _mapper = mapper;
        _repository = repository;
        _substructureRepository = substructureRepository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<SubstructureCostProfileDto> AddOrUpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        var substructure = await _substructureRepository.GetSubstructureWithCostProfile(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        if (substructure.CostProfile != null)
        {
            return await UpdateSubstructureCostProfile(caseId, substructureId, substructure.CostProfile.Id, dto);
        }

        return await CreateSubstructureCostProfile(caseId, substructureId, dto, substructure);
    }

    private async Task<SubstructureCostProfileDto> UpdateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        Guid profileId,
        UpdateSubstructureCostProfileDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfile, SubstructureCostProfileDto, UpdateSubstructureCostProfileDto>(
            caseId,
            substructureId,
            profileId,
            dto,
            _repository.GetSubstructureCostProfile,
            _repository.UpdateSubstructureCostProfile
        );
    }

    private async Task<SubstructureCostProfileDto> CreateSubstructureCostProfile(
        Guid caseId,
        Guid substructureId,
        UpdateSubstructureCostProfileDto dto,
        Substructure substructure
    )
    {
        SubstructureCostProfile substructureCostProfile = new SubstructureCostProfile
        {
            Substructure = substructure
        };

        var newProfile = _mapperService.MapToEntity(dto, substructureCostProfile, substructureId);

        try
        {
            _repository.CreateSubstructureCostProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create cost profile for substructure with id {substructureId} for case id {caseId}.", substructureId, caseId);
            throw;
        }

        var newDto = _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newProfile, newProfile.Id);
        return newDto;
    }

    public async Task<SubstructureCostProfileOverrideDto> CreateSubstructureCostProfileOverride(
        Guid caseId,
        Guid substructureId,
        CreateSubstructureCostProfileOverrideDto dto
    )
    {
        var substructure = await _substructureRepository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        var resourceHasProfile = await _substructureRepository.SubstructureHasCostProfileOverride(substructureId);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Substructure with id {substructureId} already has a profile of type {typeof(SubstructureCostProfileOverride).Name}.");
        }

        SubstructureCostProfileOverride profile = new()
        {
            Substructure = substructure,
        };

        var newProfile = _mapperService.MapToEntity(dto, profile, substructureId);

        SubstructureCostProfileOverride createdProfile;
        try
        {
            createdProfile = _repository.CreateSubstructureCostProfileOverride(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile SubstructureCostProfileOverride for case id {caseId}.", caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }

    public async Task<SubstructureCostProfileOverrideDto> UpdateSubstructureCostProfileOverride(
        Guid caseId,
        Guid substructureId,
        Guid costProfileId,
        UpdateSubstructureCostProfileOverrideDto dto
    )
    {
        return await UpdateSubstructureTimeSeries<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto, UpdateSubstructureCostProfileOverrideDto>(
        caseId,
        substructureId,
        costProfileId,
        dto,
        _repository.GetSubstructureCostProfileOverride,
        _repository.UpdateSubstructureCostProfileOverride
);
    }

    private async Task<TDto> UpdateSubstructureTimeSeries<TProfile, TDto, TUpdateDto>(
    Guid caseId,
    Guid substructureId,
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

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, substructureId);

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
