using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.Create;
using api.Features.CaseProfiles.Dtos.Update;
using api.Features.CaseProfiles.Enums;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseProfiles.Services;

public class CaseTimeSeriesService(
    DcdDbContext context,
    IMapperService mapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
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
            id => context.CessationWellsCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.CessationWellsCostOverride.Update(profile)
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
            id => context.CessationOffshoreFacilitiesCostOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.CessationOffshoreFacilitiesCostOverride.Update(profile)
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
            id => context.CessationOnshoreFacilitiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.CessationOnshoreFacilitiesCostProfile.Update(profile)
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
            id => context.TotalFeasibilityAndConceptStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.TotalFeasibilityAndConceptStudiesOverride.Update(profile)
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
            id => context.TotalFEEDStudiesOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.TotalFEEDStudiesOverride.Update(profile)
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
            id => context.TotalOtherStudiesCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.TotalOtherStudiesCostProfile.Update(profile)
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
            id => context.HistoricCostCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.HistoricCostCostProfile.Update(profile)
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
            id => context.WellInterventionCostProfileOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.WellInterventionCostProfileOverride.Update(profile)
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
            id => context.OffshoreFacilitiesOperationsCostProfileOverride.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.OffshoreFacilitiesOperationsCostProfileOverride.Update(profile)
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
            id => context.OnshoreRelatedOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.OnshoreRelatedOPEXCostProfile.Update(profile)
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
            profile => context.OffshoreFacilitiesOperationsCostProfileOverride.Add(profile),
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
            profile => context.CessationWellsCostOverride.Add(profile),
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
            profile => context.CessationOffshoreFacilitiesCostOverride.Add(profile),
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
            profile => context.CessationOnshoreFacilitiesCostProfile.Add(profile),
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
            profile => context.TotalFeasibilityAndConceptStudiesOverride.Add(profile),
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
            profile => context.TotalFEEDStudiesOverride.Add(profile),
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
            profile => context.TotalOtherStudiesCostProfile.Add(profile),
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
            profile => context.HistoricCostCostProfile.Add(profile),
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
            profile => context.WellInterventionCostProfileOverride.Add(profile),
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
            profile => context.OnshoreRelatedOPEXCostProfile.Add(profile),
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
            profile => context.AdditionalOPEXCostProfile.Add(profile),
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
            id => context.AdditionalOPEXCostProfile.Include(x => x.Case).SingleAsync(x => x.Id == id),
            profile => context.AdditionalOPEXCostProfile.Update(profile)
        );
    }

    private async Task<TDto> UpdateCaseCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        TUpdateDto updatedCostProfileDto,
        Func<Guid, Task<TProfile>> getProfile,
        Action<TProfile> updateProfile
    )
        where TProfile : class, ICaseTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(costProfileId);

        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, existingProfile.Case.Id);

        mapperService.MapToEntity(updatedCostProfileDto, existingProfile, caseId);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(existingProfile, costProfileId);
    }

    private async Task<TDto> CreateCaseProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        TCreateDto createProfileDto,
        Action<TProfile> createProfile,
        CaseProfileNames profileName
    )
        where TProfile : class, ICaseTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<Case>(projectId, caseId);

        var caseEntity = await context.Cases.SingleAsync(x => x.Id == caseId);

        var resourceHasProfile = await CaseHasProfile(caseId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Case with id {caseId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Case = caseEntity
        };

        var newProfile = mapperService.MapToEntity(createProfileDto, profile, caseId);

        createProfile(newProfile);
        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return mapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id);
    }

    private async Task<bool> CaseHasProfile(Guid caseId, CaseProfileNames profileType)
    {
        Expression<Func<Case, bool>> profileExistsExpression = profileType switch
        {
            CaseProfileNames.CessationWellsCostOverride => d => d.CessationWellsCostOverride != null,
            CaseProfileNames.CessationOffshoreFacilitiesCostOverride => d => d.CessationOffshoreFacilitiesCostOverride != null,
            CaseProfileNames.CessationOnshoreFacilitiesCostProfile => d => d.CessationOnshoreFacilitiesCostProfile != null,
            CaseProfileNames.TotalFeasibilityAndConceptStudiesOverride => d => d.TotalFeasibilityAndConceptStudiesOverride != null,
            CaseProfileNames.TotalFEEDStudiesOverride => d => d.TotalFEEDStudiesOverride != null,
            CaseProfileNames.TotalOtherStudiesCostProfile => d => d.TotalOtherStudiesCostProfile != null,
            CaseProfileNames.HistoricCostCostProfile => d => d.HistoricCostCostProfile != null,
            CaseProfileNames.WellInterventionCostProfileOverride => d => d.WellInterventionCostProfileOverride != null,
            CaseProfileNames.OffshoreFacilitiesOperationsCostProfileOverride => d => d.OffshoreFacilitiesOperationsCostProfileOverride != null,
            CaseProfileNames.OnshoreRelatedOPEXCostProfile => d => d.OnshoreRelatedOPEXCostProfile != null,
            CaseProfileNames.AdditionalOPEXCostProfile => d => d.AdditionalOPEXCostProfile != null,
            CaseProfileNames.CalculatedTotalIncomeCostProfile => d => d.CalculatedTotalIncomeCostProfile != null,
            CaseProfileNames.CalculatedTotalCostCostProfile => d => d.CalculatedTotalCostCostProfile != null,
        };

        return await context.Cases
            .Where(d => d.Id == caseId)
            .AnyAsync(profileExistsExpression);
    }
}
