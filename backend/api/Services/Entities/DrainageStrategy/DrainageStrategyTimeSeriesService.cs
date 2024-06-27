using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyTimeSeriesService : IDrainageStrategyTimeSeriesService
{
    private readonly ILogger<DrainageStrategyService> _logger;
    private readonly ICaseRepository _caseRepository;
    private readonly IDrainageStrategyTimeSeriesRepository _repository;
    private readonly IDrainageStrategyRepository _drainageStrategyRepository;
    private readonly IConversionMapperService _conversionMapperService;
    private readonly IProjectRepository _projectRepository;

    public DrainageStrategyTimeSeriesService(
        ILoggerFactory loggerFactory,
        ICaseRepository caseRepository,
        IDrainageStrategyTimeSeriesRepository repository,
        IDrainageStrategyRepository drainageStrategyTimeSeriesRepository,
        IConversionMapperService conversionMapperService,
        IProjectRepository projectRepository
        )
    {
        _logger = loggerFactory.CreateLogger<DrainageStrategyService>();
        _caseRepository = caseRepository;
        _repository = repository;
        _drainageStrategyRepository = drainageStrategyTimeSeriesRepository;
        _conversionMapperService = conversionMapperService;
        _projectRepository = projectRepository;
    }

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
            _repository.CreateProductionProfileOil,
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
            _repository.GetProductionProfileOil,
            _repository.UpdateProductionProfileOil
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
            _repository.CreateProductionProfileGas,
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
            _repository.GetProductionProfileGas,
            _repository.UpdateProductionProfileGas
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
            _repository.CreateProductionProfileWater,
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
            _repository.GetProductionProfileWater,
            _repository.UpdateProductionProfileWater
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
            _repository.CreateProductionProfileWaterInjection,
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
            _repository.GetProductionProfileWaterInjection,
            _repository.UpdateProductionProfileWaterInjection
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
            _repository.CreateFuelFlaringAndLossesOverride,
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
            _repository.GetFuelFlaringAndLossesOverride,
            _repository.UpdateFuelFlaringAndLossesOverride
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
            _repository.CreateNetSalesGasOverride,
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
            _repository.GetNetSalesGasOverride,
            _repository.UpdateNetSalesGasOverride
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
            _repository.CreateCo2EmissionsOverride,
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
            _repository.GetCo2EmissionsOverride,
            _repository.UpdateCo2EmissionsOverride
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
            _repository.CreateImportedElectricityOverride,
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
            _repository.GetImportedElectricityOverride,
            _repository.UpdateImportedElectricityOverride
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
            _repository.CreateDeferredOilProduction,
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
            _repository.GetDeferredOilProduction,
            _repository.UpdateDeferredOilProduction
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
            _repository.CreateDeferredGasProduction,
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
            _repository.GetDeferredGasProduction,
            _repository.UpdateDeferredGasProduction
        );
    }

    private async Task<TDto> UpdateDrainageStrategyProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        Guid productionProfileId,
        TUpdateDto updatedProductionProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IDrainageStrategyTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(productionProfileId)
            ?? throw new NotFoundInDBException($"Production profile with id {productionProfileId} not found.");

        var project = await _projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        _conversionMapperService.MapToEntity(updatedProductionProfileDto, existingProfile, drainageStrategyId, project.PhysicalUnit);

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
            _logger.LogError(ex, "Failed to update profile {profileName} with id {productionProfileId} for case id {caseId}.", profileName, productionProfileId, caseId);
            throw;
        }

        var updatedDto = _conversionMapperService.MapToDto<TProfile, TDto>(existingProfile, productionProfileId, project.PhysicalUnit);
        return updatedDto;
    }

    private async Task<TDto> CreateDrainageStrategyProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        TCreateDto createProductionProfileDto,
        Func<TProfile, TProfile> createProfile,
        DrainageStrategyProfileNames profileName
    )
        where TProfile : class, IDrainageStrategyTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        var drainageStrategy = await _drainageStrategyRepository.GetDrainageStrategy(drainageStrategyId)
            ?? throw new NotFoundInDBException($"Drainage strategy with id {drainageStrategyId} not found.");

        var resourceHasProfile = await _drainageStrategyRepository.DrainageStrategyHasProfile(drainageStrategyId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Drainage strategy with id {drainageStrategyId} already has a profile of type {typeof(TProfile).Name}.");
        }

        var project = await _projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        TProfile profile = new()
        {
            DrainageStrategy = drainageStrategy,
        };

        var newProfile = _conversionMapperService.MapToEntity(createProductionProfileDto, profile, drainageStrategyId, project.PhysicalUnit);

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

        var updatedDto = _conversionMapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id, project.PhysicalUnit);
        return updatedDto;
    }
}
