using System.Linq.Expressions;

using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos.Create;
using api.Features.Cases.Recalculation;
using api.Features.ProjectIntegrity;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Services;

public class DrainageStrategyTimeSeriesService(
    DcdDbContext context,
    IConversionMapperService conversionMapperService,
    IProjectIntegrityService projectIntegrityService,
    IRecalculationService recalculationService)
{
    public async Task<ProductionProfileOilDto> CreateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileOilDto createProductionProfileOilDto
    )
    {
        return await CreateDrainageStrategyProfile<ProductionProfileOil, ProductionProfileOilDto, CreateProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProductionProfileOilDto,
            DrainageStrategyProfileNames.ProductionProfileOil
        );
    }

    public async Task<ProductionProfileOilDto> UpdateProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileOilId,
        UpdateProductionProfileOilDto updatedProductionProfileOilDto
    )
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileOil, ProductionProfileOilDto, UpdateProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileOilId,
            updatedProductionProfileOilDto,
            id => context.ProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<AdditionalProductionProfileOilDto> CreateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateAdditionalProductionProfileOilDto createAdditionalProductionProfileOilDto
    )
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileOil, AdditionalProductionProfileOilDto, CreateAdditionalProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createAdditionalProductionProfileOilDto,
            DrainageStrategyProfileNames.AdditionalProductionProfileOil
        );
    }

    public async Task<AdditionalProductionProfileOilDto> UpdateAdditionalProductionProfileOil(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid additionalProductionProfileOilId,
        UpdateAdditionalProductionProfileOilDto updatedAdditionalProductionProfileOilDto
    )
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileOil, AdditionalProductionProfileOilDto, UpdateAdditionalProductionProfileOilDto>(
            projectId,
            caseId,
            drainageStrategyId,
            additionalProductionProfileOilId,
            updatedAdditionalProductionProfileOilDto,
            id => context.AdditionalProductionProfileOil.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<ProductionProfileGasDto> CreateProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileGasDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<ProductionProfileGas, ProductionProfileGasDto, CreateProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.ProductionProfileGas
        );
    }

    public async Task<ProductionProfileGasDto> UpdateProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileGasDto updatedProductionProfileGasDto
    )
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileGas, ProductionProfileGasDto, UpdateProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedProductionProfileGasDto,
            id => context.ProductionProfileGas.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<AdditionalProductionProfileGasDto> CreateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateAdditionalProductionProfileGasDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<AdditionalProductionProfileGas, AdditionalProductionProfileGasDto, CreateAdditionalProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.AdditionalProductionProfileGas
        );
    }

    public async Task<AdditionalProductionProfileGasDto> UpdateAdditionalProductionProfileGas(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateAdditionalProductionProfileGasDto updatedAdditionalProductionProfileGasDto
    )
    {
        return await UpdateDrainageStrategyProfile<AdditionalProductionProfileGas, AdditionalProductionProfileGasDto, UpdateAdditionalProductionProfileGasDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedAdditionalProductionProfileGasDto,
            id => context.AdditionalProductionProfileGas.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<ProductionProfileWaterDto> CreateProductionProfileWater(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileWaterDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<ProductionProfileWater, ProductionProfileWaterDto, CreateProductionProfileWaterDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.ProductionProfileWater
        );
    }

    public async Task<ProductionProfileWaterDto> UpdateProductionProfileWater(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileWaterDto updatedProductionProfileWaterDto
    )
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileWater, ProductionProfileWaterDto, UpdateProductionProfileWaterDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedProductionProfileWaterDto,
            id => context.ProductionProfileWater.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<ProductionProfileWaterInjectionDto> CreateProductionProfileWaterInjection(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateProductionProfileWaterInjectionDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto, CreateProductionProfileWaterInjectionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.ProductionProfileWaterInjection
        );
    }

    public async Task<ProductionProfileWaterInjectionDto> UpdateProductionProfileWaterInjection(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateProductionProfileWaterInjectionDto updatedProductionProfileWaterInjectionDto
    )
    {
        return await UpdateDrainageStrategyProfile<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto, UpdateProductionProfileWaterInjectionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedProductionProfileWaterInjectionDto,
            id => context.ProductionProfileWaterInjection.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<FuelFlaringAndLossesOverrideDto> CreateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateFuelFlaringAndLossesOverrideDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto, CreateFuelFlaringAndLossesOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.FuelFlaringAndLossesOverride
        );
    }

    public async Task<FuelFlaringAndLossesOverrideDto> UpdateFuelFlaringAndLossesOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateFuelFlaringAndLossesOverrideDto updateDto
    )
    {
        return await UpdateDrainageStrategyProfile<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto, UpdateFuelFlaringAndLossesOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => context.FuelFlaringAndLossesOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<NetSalesGasOverrideDto> CreateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateNetSalesGasOverrideDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<NetSalesGasOverride, NetSalesGasOverrideDto, CreateNetSalesGasOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.NetSalesGasOverride
        );
    }

    public async Task<NetSalesGasOverrideDto> UpdateNetSalesGasOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateNetSalesGasOverrideDto updateDto
    )
    {
        return await UpdateDrainageStrategyProfile<NetSalesGasOverride, NetSalesGasOverrideDto, UpdateNetSalesGasOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => context.NetSalesGasOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<Co2EmissionsOverrideDto> CreateCo2EmissionsOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateCo2EmissionsOverrideDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<Co2EmissionsOverride, Co2EmissionsOverrideDto, CreateCo2EmissionsOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.Co2EmissionsOverride
        );
    }

    public async Task<Co2EmissionsOverrideDto> UpdateCo2EmissionsOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateCo2EmissionsOverrideDto updateDto
    )
    {
        return await UpdateDrainageStrategyProfile<Co2EmissionsOverride, Co2EmissionsOverrideDto, UpdateCo2EmissionsOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => context.Co2EmissionsOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<ImportedElectricityOverrideDto> CreateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateImportedElectricityOverrideDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<ImportedElectricityOverride, ImportedElectricityOverrideDto, CreateImportedElectricityOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.ImportedElectricityOverride
        );
    }

    public async Task<ImportedElectricityOverrideDto> UpdateImportedElectricityOverride(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid profileId,
        UpdateImportedElectricityOverrideDto updateDto
    )
    {
        return await UpdateDrainageStrategyProfile<ImportedElectricityOverride, ImportedElectricityOverrideDto, UpdateImportedElectricityOverrideDto>(
            projectId,
            caseId,
            drainageStrategyId,
            profileId,
            updateDto,
            id => context.ImportedElectricityOverride.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<DeferredOilProductionDto> CreateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateDeferredOilProductionDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<DeferredOilProduction, DeferredOilProductionDto, CreateDeferredOilProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.DeferredOilProduction
        );
    }

    public async Task<DeferredOilProductionDto> UpdateDeferredOilProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateDeferredOilProductionDto updatedDeferredOilProductionDto
    )
    {
        return await UpdateDrainageStrategyProfile<DeferredOilProduction, DeferredOilProductionDto, UpdateDeferredOilProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredOilProductionDto,
            id => context.DeferredOilProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    public async Task<DeferredGasProductionDto> CreateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        CreateDeferredGasProductionDto createProfileDto
    )
    {
        return await CreateDrainageStrategyProfile<DeferredGasProduction, DeferredGasProductionDto, CreateDeferredGasProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            createProfileDto,
            DrainageStrategyProfileNames.DeferredGasProduction
        );
    }

    public async Task<DeferredGasProductionDto> UpdateDeferredGasProduction(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        UpdateDeferredGasProductionDto updatedDeferredGasProductionDto
    )
    {
        return await UpdateDrainageStrategyProfile<DeferredGasProduction, DeferredGasProductionDto, UpdateDeferredGasProductionDto>(
            projectId,
            caseId,
            drainageStrategyId,
            productionProfileId,
            updatedDeferredGasProductionDto,
            id => context.DeferredGasProduction.Include(x => x.DrainageStrategy).SingleAsync(x => x.Id == id)
        );
    }

    private async Task<TDto> UpdateDrainageStrategyProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        TUpdateDto updatedProductionProfileDto,
        Func<Guid, Task<TProfile>> getProfile
    )
        where TProfile : class, IDrainageStrategyTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(productionProfileId);

        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        await projectIntegrityService.EntityIsConnectedToProject<DrainageStrategy>(projectPk, existingProfile.DrainageStrategy.Id);

        var project = await context.Projects.SingleAsync(p => p.Id == projectPk);

        conversionMapperService.MapToEntity(updatedProductionProfileDto, existingProfile, drainageStrategyId, project.PhysicalUnit);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return conversionMapperService.MapToDto<TProfile, TDto>(existingProfile, productionProfileId, project.PhysicalUnit);
    }

    private async Task<TDto> CreateDrainageStrategyProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        TCreateDto createProductionProfileDto,
        DrainageStrategyProfileNames profileName
    )
        where TProfile : class, IDrainageStrategyTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        await projectIntegrityService.EntityIsConnectedToProject<DrainageStrategy>(projectId, drainageStrategyId);

        var drainageStrategy = await context.DrainageStrategies.SingleAsync(x => x.Id == drainageStrategyId);

        var resourceHasProfile = await DrainageStrategyHasProfile(drainageStrategyId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Drainage strategy with id {drainageStrategyId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var project = await context.Projects.SingleAsync(p => p.Id == projectId);

        TProfile profile = new()
        {
            DrainageStrategy = drainageStrategy
        };

        var newProfile = conversionMapperService.MapToEntity(createProductionProfileDto, profile, drainageStrategyId, project.PhysicalUnit);

        context.Add(newProfile);

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return conversionMapperService.MapToDto<TProfile, TDto>(newProfile, newProfile.Id, project.PhysicalUnit);
    }

    private async Task<bool> DrainageStrategyHasProfile(Guid drainageStrategyId, DrainageStrategyProfileNames profileType)
    {
        Expression<Func<DrainageStrategy, bool>> profileExistsExpression = profileType switch
        {
            DrainageStrategyProfileNames.ProductionProfileOil => d => d.ProductionProfileOil != null,
            DrainageStrategyProfileNames.AdditionalProductionProfileOil => d => d.AdditionalProductionProfileOil != null,
            DrainageStrategyProfileNames.ProductionProfileGas => d => d.ProductionProfileGas != null,
            DrainageStrategyProfileNames.AdditionalProductionProfileGas => d => d.AdditionalProductionProfileGas != null,
            DrainageStrategyProfileNames.ProductionProfileWater => d => d.ProductionProfileWater != null,
            DrainageStrategyProfileNames.ProductionProfileWaterInjection => d => d.ProductionProfileWaterInjection != null,
            DrainageStrategyProfileNames.FuelFlaringAndLossesOverride => d => d.FuelFlaringAndLossesOverride != null,
            DrainageStrategyProfileNames.NetSalesGasOverride => d => d.NetSalesGasOverride != null,
            DrainageStrategyProfileNames.Co2EmissionsOverride => d => d.Co2EmissionsOverride != null,
            DrainageStrategyProfileNames.ImportedElectricityOverride => d => d.ImportedElectricityOverride != null,
            DrainageStrategyProfileNames.DeferredOilProduction => d => d.DeferredOilProduction != null,
            DrainageStrategyProfileNames.DeferredGasProduction => d => d.DeferredGasProduction != null,
        };

        return await context.DrainageStrategies
            .Where(d => d.Id == drainageStrategyId)
            .AnyAsync(profileExistsExpression);
    }
}
