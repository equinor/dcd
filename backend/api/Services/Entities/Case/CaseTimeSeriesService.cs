using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseTimeSeriesService : ICaseTimeSeriesService
{
    private readonly ILogger<CaseService> _logger;
    private readonly IMapperService _mapperService;
    private readonly ICaseTimeSeriesRepository _repository;
    private readonly ICaseRepository _caseRepository;

    public CaseTimeSeriesService(
        ILoggerFactory loggerFactory,
        ICaseTimeSeriesRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
    )
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _mapperService = mapperService;
        _repository = repository;
        _caseRepository = caseRepository;
    }

    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationWellsCostOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, UpdateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetCessationWellsCostOverride,
            _repository.UpdateCessationWellsCostOverride
        );
    }

    public async Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationOffshoreFacilitiesCostOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, UpdateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetCessationOffshoreFacilitiesCostOverride,
            _repository.UpdateCessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFeasibilityAndConceptStudiesOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, UpdateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetTotalFeasibilityAndConceptStudiesOverride,
            _repository.UpdateTotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> UpdateTotalFEEDStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFEEDStudiesOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto, UpdateTotalFEEDStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetTotalFEEDStudiesOverride,
            _repository.UpdateTotalFEEDStudiesOverride
        );
    }

    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateHistoricCostCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, UpdateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetHistoricCostCostProfile,
            _repository.UpdateHistoricCostCostProfile
        );
    }

    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateWellInterventionCostProfileOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, UpdateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetWellInterventionCostProfileOverride,
            _repository.UpdateWellInterventionCostProfileOverride
        );
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetOffshoreFacilitiesOperationsCostProfileOverride,
            _repository.UpdateOffshoreFacilitiesOperationsCostProfileOverride
        );
    }

    public async Task<OnshoreRelatedOPEXCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOnshoreRelatedOPEXCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto, UpdateOnshoreRelatedOPEXCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetOnshoreRelatedOPEXCostProfile,
            _repository.UpdateOnshoreRelatedOPEXCostProfile
        );
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateOffshoreFacilitiesOperationsCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, CreateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateOffshoreFacilitiesOperationsCostProfileOverride,
            CaseProfileNames.OffshoreFacilitiesOperationsCostProfileOverride
        );
    }
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        CreateCessationWellsCostOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, CreateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateCessationWellsCostOverride,
            CaseProfileNames.CessationWellsCostOverride
        );
    }

    public async Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        CreateCessationOffshoreFacilitiesCostOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, CreateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateCessationOffshoreFacilitiesCostOverride,
            CaseProfileNames.CessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFeasibilityAndConceptStudiesOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, CreateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateTotalFeasibilityAndConceptStudiesOverride,
            CaseProfileNames.TotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> CreateTotalFEEDStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFEEDStudiesOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto, CreateTotalFEEDStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateTotalFEEDStudiesOverride,
            CaseProfileNames.TotalFEEDStudiesOverride
        );
    }

    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        CreateHistoricCostCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, CreateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateHistoricCostCostProfile,
            CaseProfileNames.HistoricCostCostProfile
        );
    }

    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateWellInterventionCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, CreateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateWellInterventionCostProfileOverride,
            CaseProfileNames.WellInterventionCostProfileOverride
        );
    }

    public async Task<OnshoreRelatedOPEXCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        CreateOnshoreRelatedOPEXCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto, CreateOnshoreRelatedOPEXCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateOnshoreRelatedOPEXCostProfile,
            CaseProfileNames.OnshoreRelatedOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> CreateAdditionalOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        CreateAdditionalOPEXCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto, CreateAdditionalOPEXCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateAdditionalOPEXCostProfile,
            CaseProfileNames.AdditionalOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateAdditionalOPEXCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto, UpdateAdditionalOPEXCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetAdditionalOPEXCostProfile,
            _repository.UpdateAdditionalOPEXCostProfile
        );
    }

    private async Task<TDto> UpdateCaseCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        TUpdateDto updatedCostProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(costProfileId)
            ?? throw new NotFoundInDBException($"Production profile with id {costProfileId} not found.");

        _mapperService.MapToEntity(updatedCostProfileDto, existingProfile, caseId);

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
            _logger.LogError(ex, "Failed to update profile {profileName} with id {costProfileId} for case id {caseId}.", profileName, costProfileId, caseId);
            throw;
        }


        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, costProfileId);
        return updatedDto;
    }

    private async Task<TDto> CreateCaseProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        TCreateDto createProfileDto,
        Func<TProfile, TProfile> createProfile,
        CaseProfileNames profileName
    )
        where TProfile : class, ICaseTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        var caseEntity = await _caseRepository.GetCase(caseId)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");

        var resourceHasProfile = await _caseRepository.CaseHasProfile(caseId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Case with id {caseId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Case = caseEntity,
        };

        var newProfile = _mapperService.MapToEntity(createProfileDto, profile, caseId);

        TProfile createdProfile;
        try
        {
            createdProfile = createProfile(newProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
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
