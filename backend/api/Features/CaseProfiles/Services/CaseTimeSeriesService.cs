using api.Exceptions;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Create;
using api.Features.CaseProfiles.Dtos.Update;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

namespace api.Features.CaseProfiles.Services;

public class CaseTimeSeriesService(
    ICaseTimeSeriesRepository repository,
    ICaseRepository caseRepository,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
    : ICaseTimeSeriesService
{
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
            repository.GetCessationWellsCostOverride,
            repository.UpdateCessationWellsCostOverride
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
            repository.GetCessationOffshoreFacilitiesCostOverride,
            repository.UpdateCessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<CessationOnshoreFacilitiesCostProfileDto> UpdateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationOnshoreFacilitiesCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto, UpdateCessationOnshoreFacilitiesCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            repository.GetCessationOnshoreFacilitiesCostProfile,
            repository.UpdateCessationOnshoreFacilitiesCostProfile
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
            repository.GetTotalFeasibilityAndConceptStudiesOverride,
            repository.UpdateTotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> UpdateTotalFeedStudiesOverride(
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
            repository.GetTotalFEEDStudiesOverride,
            repository.UpdateTotalFEEDStudiesOverride
        );
    }

    public async Task<TotalOtherStudiesCostProfileDto> UpdateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalOtherStudiesCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto, UpdateTotalOtherStudiesCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            repository.GetTotalOtherStudiesCostProfile,
            repository.UpdateTotalOtherStudiesCostProfile
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
            repository.GetHistoricCostCostProfile,
            repository.UpdateHistoricCostCostProfile
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
            repository.GetWellInterventionCostProfileOverride,
            repository.UpdateWellInterventionCostProfileOverride
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
            repository.GetOffshoreFacilitiesOperationsCostProfileOverride,
            repository.UpdateOffshoreFacilitiesOperationsCostProfileOverride
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
            repository.GetOnshoreRelatedOPEXCostProfile,
            repository.UpdateOnshoreRelatedOPEXCostProfile
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
            repository.CreateOffshoreFacilitiesOperationsCostProfileOverride,
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
            repository.CreateCessationWellsCostOverride,
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
            repository.CreateCessationOffshoreFacilitiesCostOverride,
            CaseProfileNames.CessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<CessationOnshoreFacilitiesCostProfileDto> CreateCessationOnshoreFacilitiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateCessationOnshoreFacilitiesCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto, CreateCessationOnshoreFacilitiesCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            repository.CreateCessationOnshoreFacilitiesCostProfile,
            CaseProfileNames.CessationOnshoreFacilitiesCostProfile
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
            repository.CreateTotalFeasibilityAndConceptStudiesOverride,
            CaseProfileNames.TotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> CreateTotalFeedStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFEEDStudiesOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto, CreateTotalFEEDStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            repository.CreateTotalFEEDStudiesOverride,
            CaseProfileNames.TotalFEEDStudiesOverride
        );
    }

    public async Task<TotalOtherStudiesCostProfileDto> CreateTotalOtherStudiesCostProfile(
        Guid projectId,
        Guid caseId,
        CreateTotalOtherStudiesCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto, CreateTotalOtherStudiesCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            repository.CreateTotalOtherStudiesCostProfile,
            CaseProfileNames.TotalOtherStudiesCostProfile
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
            repository.CreateHistoricCostCostProfile,
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
            repository.CreateWellInterventionCostProfileOverride,
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
            repository.CreateOnshoreRelatedOPEXCostProfile,
            CaseProfileNames.OnshoreRelatedOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> CreateAdditionalOpexCostProfile(
        Guid projectId,
        Guid caseId,
        CreateAdditionalOPEXCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto, CreateAdditionalOPEXCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            repository.CreateAdditionalOPEXCostProfile,
            CaseProfileNames.AdditionalOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOpexCostProfile(
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
            repository.GetAdditionalOPEXCostProfile,
            repository.UpdateAdditionalOPEXCostProfile
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
        where TProfile : class, ICaseTimeSeries
        where TDto : class
        where TUpdateDto : class
    {

        var existingProfile = await getProfile(costProfileId)
            ?? throw new NotFoundInDbException($"Production profile with id {costProfileId} not found.");

        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, existingProfile.Case.Id);

        mapperService.MapToEntity(updatedCostProfileDto, existingProfile, caseId);

        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(existingProfile, costProfileId);
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
        // Need to verify that the project from the URL is the same as the project of the resource
        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, caseId);

        var caseEntity = await caseRepository.GetCase(caseId)
            ?? throw new NotFoundInDbException($"Case with id {caseId} not found.");

        var resourceHasProfile = await caseRepository.CaseHasProfile(caseId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Case with id {caseId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Case = caseEntity,
        };

        var newProfile = mapperService.MapToEntity(createProfileDto, profile, caseId);

        var createdProfile = createProfile(newProfile);
        await caseRepository.UpdateModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        var updatedDto = mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }
}
