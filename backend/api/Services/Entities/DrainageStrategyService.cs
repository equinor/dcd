using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService : IDrainageStrategyService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<DrainageStrategyService> _logger;
    private readonly IMapper _mapper;
    private readonly ICaseRepository _caseRepository;
    private readonly IDrainageStrategyRepository _repository;
    private readonly IConversionMapperService _conversionMapperService;
    private readonly IProjectRepository _projectRepository;

    public DrainageStrategyService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ICaseRepository caseRepository,
        IDrainageStrategyRepository repository,
        IConversionMapperService conversionMapperService,
        IProjectRepository projectRepository
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<DrainageStrategyService>();
        _mapper = mapper;
        _caseRepository = caseRepository;
        _repository = repository;
        _conversionMapperService = conversionMapperService;
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto> CreateDrainageStrategy(DrainageStrategyWithProfilesDto drainageStrategyDto, Guid sourceCaseId)
    {
        var unit = (await _projectService.GetProject(drainageStrategyDto.ProjectId)).PhysicalUnit;
        var drainageStrategy = _mapper.Map<DrainageStrategy>(drainageStrategyDto);
        if (drainageStrategy == null)
        {
            throw new Exception("Drainage stragegy null");
        }
        var project = await _projectService.GetProject(drainageStrategy.ProjectId);
        drainageStrategy.Project = project;
        _context.DrainageStrategies!.Add(drainageStrategy);
        await _context.SaveChangesAsync();
        await SetCaseLink(drainageStrategy, sourceCaseId, project);
        return await _projectService.GetProjectDto(drainageStrategy.ProjectId);
    }

    public async Task<DrainageStrategy> NewCreateDrainageStrategy(Guid projectId, Guid sourceCaseId, CreateDrainageStrategyDto drainageStrategyDto)
    {
        var drainageStrategy = _mapper.Map<DrainageStrategy>(drainageStrategyDto);
        if (drainageStrategy == null)
        {
            throw new ArgumentNullException(nameof(drainageStrategy));
        }
        var project = await _projectService.GetProject(projectId);
        drainageStrategy.Project = project;
        var createdDrainageStrategy = _context.DrainageStrategies!.Add(drainageStrategy);
        await _context.SaveChangesAsync();
        await SetCaseLink(drainageStrategy, sourceCaseId, project);
        return createdDrainageStrategy.Entity;
    }

    public async Task<DrainageStrategyWithProfilesDto> CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId)
    {
        var source = await GetDrainageStrategy(drainageStrategyId);
        var unit = (await _projectService.GetProject(source.ProjectId)).PhysicalUnit;

        var newDrainageStrategyDto = _mapper.Map<DrainageStrategy, DrainageStrategyWithProfilesDto>(
            source,
            opts => opts.Items["ConversionUnit"] = unit.ToString()
        );

        if (newDrainageStrategyDto == null)
        {
            throw new Exception();
        }

        newDrainageStrategyDto.Id = Guid.Empty;
        if (newDrainageStrategyDto.ProductionProfileOil != null)
        {
            newDrainageStrategyDto.ProductionProfileOil.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ProductionProfileGas != null)
        {
            newDrainageStrategyDto.ProductionProfileGas.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ProductionProfileWater != null)
        {
            newDrainageStrategyDto.ProductionProfileWater.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ProductionProfileWaterInjection != null)
        {
            newDrainageStrategyDto.ProductionProfileWaterInjection.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.FuelFlaringAndLosses != null)
        {
            newDrainageStrategyDto.FuelFlaringAndLosses.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.FuelFlaringAndLossesOverride != null)
        {
            newDrainageStrategyDto.FuelFlaringAndLossesOverride.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.NetSalesGas != null)
        {
            newDrainageStrategyDto.NetSalesGas.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.NetSalesGasOverride != null)
        {
            newDrainageStrategyDto.NetSalesGasOverride.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.Co2Emissions != null)
        {
            newDrainageStrategyDto.Co2Emissions.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.Co2EmissionsOverride != null)
        {
            newDrainageStrategyDto.Co2EmissionsOverride.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ProductionProfileNGL != null)
        {
            newDrainageStrategyDto.ProductionProfileNGL.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ImportedElectricity != null)
        {
            newDrainageStrategyDto.ImportedElectricity.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.ImportedElectricityOverride != null)
        {
            newDrainageStrategyDto.ImportedElectricityOverride.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.DeferredOilProduction != null)
        {
            newDrainageStrategyDto.DeferredOilProduction.Id = Guid.Empty;
        }
        if (newDrainageStrategyDto.DeferredGasProduction != null)
        {
            newDrainageStrategyDto.DeferredGasProduction.Id = Guid.Empty;
        }

        // var drainageStrategy = await NewCreateDrainageStrategy(newDrainageStrategyDto, sourceCaseId);
        // var dto = DrainageStrategyDtoAdapter.Convert(drainageStrategy, unit);

        // return dto;
        return new DrainageStrategyWithProfilesDto();
    }

    private async Task SetCaseLink(DrainageStrategy drainageStrategy, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.DrainageStrategyLink = drainageStrategy.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<DrainageStrategy> GetDrainageStrategy(Guid drainageStrategyId)
    {
        var drainageStrategy = await _context.DrainageStrategies!
            .Include(c => c.Project)
            .Include(c => c.ProductionProfileOil)
            .Include(c => c.ProductionProfileGas)
            .Include(c => c.ProductionProfileWater)
            .Include(c => c.ProductionProfileWaterInjection)
            .Include(c => c.FuelFlaringAndLosses)
            .Include(c => c.FuelFlaringAndLossesOverride)
            .Include(c => c.NetSalesGas)
            .Include(c => c.NetSalesGasOverride)
            .Include(c => c.Co2Emissions)
            .Include(c => c.Co2EmissionsOverride)
            .Include(c => c.ProductionProfileNGL)
            .Include(c => c.ImportedElectricity)
            .Include(c => c.ImportedElectricityOverride)
            .Include(c => c.DeferredOilProduction)
            .Include(c => c.DeferredGasProduction)
            .FirstOrDefaultAsync(o => o.Id == drainageStrategyId);
        if (drainageStrategy == null)
        {
            throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
        }
        return drainageStrategy;
    }

    public async Task<DrainageStrategyDto> UpdateDrainageStrategy(
        Guid projectId,
        Guid caseId,
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto updatedDrainageStrategyDto
    )
    {
        var existingDrainageStrategy = await _repository.GetDrainageStrategy(drainageStrategyId)
            ?? throw new NotFoundInDBException($"Drainage strategy with id {drainageStrategyId} not found.");

        var project = await _projectRepository.GetProject(projectId)
            ?? throw new NotFoundInDBException($"Project with id {projectId} not found.");

        _conversionMapperService.MapToEntity(updatedDrainageStrategyDto, existingDrainageStrategy, drainageStrategyId, project.PhysicalUnit);

        DrainageStrategy updatedDrainageStrategy;
        try
        {
            updatedDrainageStrategy = await _repository.UpdateDrainageStrategy(existingDrainageStrategy);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update drainage strategy with id {drainageStrategyId} for case id {caseId}.", drainageStrategyId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var dto = _conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(updatedDrainageStrategy, drainageStrategyId, project.PhysicalUnit);
        return dto;
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
        Func<TProfile, Task<TProfile>> updateProfile
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

        TProfile updatedProfile;
        try
        {
            updatedProfile = await updateProfile(existingProfile);
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {productionProfileId} for case id {caseId}.", profileName, productionProfileId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var updatedDto = _conversionMapperService.MapToDto<TProfile, TDto>(updatedProfile, productionProfileId, project.PhysicalUnit);
        return updatedDto;
    }
}
