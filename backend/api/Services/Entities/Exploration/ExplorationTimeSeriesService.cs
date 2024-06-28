using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class ExplorationTimeSeriesService : IExplorationTimeSeriesService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;

    private readonly ILogger<ExplorationService> _logger;
    private readonly IMapper _mapper;
    private readonly ICaseRepository _caseRepository;
    private readonly IExplorationTimeSeriesRepository _repository;
    private readonly IExplorationRepository _explorationRepository;
    private readonly IMapperService _mapperService;

    public ExplorationTimeSeriesService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ICaseRepository caseRepository,
        IExplorationTimeSeriesRepository repository,
        IExplorationRepository explorationRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<ExplorationService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = repository;
        _explorationRepository = explorationRepository;
        _mapperService = mapperService;
    }
    public async Task<GAndGAdminCostOverrideDto> CreateGAndGAdminCostOverride(
            Guid caseId,
            Guid explorationId,
            CreateGAndGAdminCostOverrideDto createProfileDto
        )
    {
        return await CreateExplorationProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, CreateGAndGAdminCostOverrideDto>(
            caseId,
            explorationId,
            createProfileDto,
            _repository.CreateGAndGAdminCostOverride,
            ExplorationProfileNames.GAndGAdminCostOverride
        );
    }
    public async Task<GAndGAdminCostOverrideDto> UpdateGAndGAdminCostOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGAndGAdminCostOverrideDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<GAndGAdminCostOverride, GAndGAdminCostOverrideDto, UpdateGAndGAdminCostOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGAndGAdminCostOverride,
            _repository.UpdateGAndGAdminCostOverride
        );
    }
    public async Task<SeismicAcquisitionAndProcessingDto> UpdateSeismicAcquisitionAndProcessing(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateSeismicAcquisitionAndProcessingDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, UpdateSeismicAcquisitionAndProcessingDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetSeismicAcquisitionAndProcessing,
            _repository.UpdateSeismicAcquisitionAndProcessing
        );
    }

    public async Task<CountryOfficeCostDto> UpdateCountryOfficeCost(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateCountryOfficeCostDto updateDto
    )
    {
        return await UpdateExplorationCostProfile<CountryOfficeCost, CountryOfficeCostDto, UpdateCountryOfficeCostDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetCountryOfficeCost,
            _repository.UpdateCountryOfficeCost
        );
    }

    public async Task<SeismicAcquisitionAndProcessingDto> CreateSeismicAcquisitionAndProcessing(
        Guid caseId,
        Guid explorationId,
        CreateSeismicAcquisitionAndProcessingDto createProfileDto
    )
    {
        return await CreateExplorationProfile<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto, CreateSeismicAcquisitionAndProcessingDto>(
            caseId,
            explorationId,
            createProfileDto,
            _repository.CreateSeismicAcquisitionAndProcessing,
            ExplorationProfileNames.SeismicAcquisitionAndProcessing
        );
    }

    public async Task<CountryOfficeCostDto> CreateCountryOfficeCost(
        Guid caseId,
        Guid explorationId,
        CreateCountryOfficeCostDto createProfileDto
    )
    {
        return await CreateExplorationProfile<CountryOfficeCost, CountryOfficeCostDto, CreateCountryOfficeCostDto>(
            caseId,
            explorationId,
            createProfileDto,
            _repository.CreateCountryOfficeCost,
            ExplorationProfileNames.CountryOfficeCost
        );
    }

    private async Task<TDto> UpdateExplorationCostProfile<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid explorationId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IExplorationTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, explorationId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
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

    private async Task<TDto> CreateExplorationProfile<TProfile, TDto, TCreateDto>(
            Guid caseId,
            Guid explorationId,
            TCreateDto createExplorationProfileDto,
            Func<TProfile, TProfile> createProfile,
            ExplorationProfileNames profileName
        )
            where TProfile : class, IExplorationTimeSeries, new()
            where TDto : class
            where TCreateDto : class
    {
        var exploration = await _explorationRepository.GetExploration(explorationId)
            ?? throw new NotFoundInDBException($"Exploration with id {explorationId} not found.");

        var resourceHasProfile = await _explorationRepository.ExplorationHasProfile(explorationId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Exploration with id {explorationId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Exploration = exploration,
        };

        var newProfile = _mapperService.MapToEntity(createExplorationProfileDto, profile, explorationId);

        TProfile createdProfile;
        try
        {
            createdProfile = createProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAndRecalculateAsync(caseId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile {profileName} for case id {caseId}.", profileName, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }
}
