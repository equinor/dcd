using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory) : IDrainageStrategyService
{
    private readonly DcdDbContext _context = context;
    private readonly IProjectService _projectService = projectService;
    private readonly ILogger<DrainageStrategyService> _logger = loggerFactory.CreateLogger<DrainageStrategyService>();

    public async Task<ProjectDto> CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
    {
        var unit = (await _projectService.GetProject(drainageStrategyDto.ProjectId)).PhysicalUnit;
        var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto, unit, true);
        var project = await _projectService.GetProject(drainageStrategy.ProjectId);
        drainageStrategy.Project = project;
        await _context.DrainageStrategies!.AddAsync(drainageStrategy);
        await _context.SaveChangesAsync();
        await SetCaseLink(drainageStrategy, sourceCaseId, project);
        return await _projectService.GetProjectDto(drainageStrategy.ProjectId);
    }

    public async Task<DrainageStrategy> NewCreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
    {
        var unit = (await _projectService.GetProject(drainageStrategyDto.ProjectId)).PhysicalUnit;
        var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto, unit, true);
        var project = await _projectService.GetProject(drainageStrategy.ProjectId);
        drainageStrategy.Project = project;
        var createdDrainageStrategy = await _context.DrainageStrategies!.AddAsync(drainageStrategy);
        await _context.SaveChangesAsync();
        await SetCaseLink(drainageStrategy, sourceCaseId, project);
        return createdDrainageStrategy.Entity;
    }

    public async Task<DrainageStrategyDto> CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId)
    {
        var source = await GetDrainageStrategy(drainageStrategyId);
        var unit = (await _projectService.GetProject(source.ProjectId)).PhysicalUnit;

        var newDrainageStrategyDto = DrainageStrategyDtoAdapter.Convert(source, unit);
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

        var drainageStrategy = await NewCreateDrainageStrategy(newDrainageStrategyDto, sourceCaseId);
        var dto = DrainageStrategyDtoAdapter.Convert(drainageStrategy, unit);

        return dto;
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

    public async Task<ProjectDto> DeleteDrainageStrategy(Guid drainageStrategyId)
    {
        var drainageStrategy = await GetDrainageStrategy(drainageStrategyId);
        _context.DrainageStrategies!.Remove(drainageStrategy);
        DeleteCaseLinks(drainageStrategyId);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(drainageStrategy.ProjectId);
    }

    private void DeleteCaseLinks(Guid drainageStrategyId)
    {
        foreach (Case c in _context.Cases!)
        {
            if (c.DrainageStrategyLink == drainageStrategyId)
            {
                c.DrainageStrategyLink = Guid.Empty;
            }
        }
    }

    public async Task<ProjectDto> UpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existing = await GetDrainageStrategy(updatedDrainageStrategyDto.Id);
        var unit = (await _projectService.GetProject(existing.ProjectId)).PhysicalUnit;

        DrainageStrategyAdapter.ConvertExisting(existing, updatedDrainageStrategyDto, unit, false);

        _context.DrainageStrategies!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
    }

    public async Task<DrainageStrategyDto> NewUpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existing = await GetDrainageStrategy(updatedDrainageStrategyDto.Id);
        var unit = (await _projectService.GetProject(existing.ProjectId)).PhysicalUnit;

        DrainageStrategyAdapter.ConvertExisting(existing, updatedDrainageStrategyDto, unit, false);

        var updatedDrainageStrategy = _context.DrainageStrategies!.Update(existing);
        await _context.SaveChangesAsync();
        return DrainageStrategyDtoAdapter.Convert(updatedDrainageStrategy.Entity, unit);
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
            .FirstOrDefaultAsync(o => o.Id == drainageStrategyId);
        if (drainageStrategy == null)
        {
            throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
        }
        return drainageStrategy;
    }
}
