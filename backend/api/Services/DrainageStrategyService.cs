using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService : IDrainageStrategyService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<DrainageStrategyService> _logger;

    public DrainageStrategyService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<DrainageStrategyService>();
    }

    public ProjectDto CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
    {
        var unit = _projectService.GetProject(drainageStrategyDto.ProjectId).PhysicalUnit;
        var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto, unit, true);
        var project = _projectService.GetProject(drainageStrategy.ProjectId);
        drainageStrategy.Project = project;
        _context.DrainageStrategies!.Add(drainageStrategy);
        _context.SaveChanges();
        SetCaseLink(drainageStrategy, sourceCaseId, project);
        return _projectService.GetProjectDto(drainageStrategy.ProjectId);
    }

    public DrainageStrategy NewCreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
    {
        var unit = _projectService.GetProject(drainageStrategyDto.ProjectId).PhysicalUnit;
        var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto, unit, true);
        var project = _projectService.GetProject(drainageStrategy.ProjectId);
        drainageStrategy.Project = project;
        var createdDrainageStrategy = _context.DrainageStrategies!.Add(drainageStrategy);
        _context.SaveChanges();
        SetCaseLink(drainageStrategy, sourceCaseId, project);
        return createdDrainageStrategy.Entity;
    }

    public DrainageStrategyDto CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId)
    {
        var source = GetDrainageStrategy(drainageStrategyId);
        var unit = _projectService.GetProject(source.ProjectId).PhysicalUnit;

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

        var drainageStrategy = NewCreateDrainageStrategy(newDrainageStrategyDto, sourceCaseId);
        var dto = DrainageStrategyDtoAdapter.Convert(drainageStrategy, unit);

        return dto;
    }

    private void SetCaseLink(DrainageStrategy drainageStrategy, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.DrainageStrategyLink = drainageStrategy.Id;
        _context.SaveChanges();
    }

    public ProjectDto DeleteDrainageStrategy(Guid drainageStrategyId)
    {
        var drainageStrategy = GetDrainageStrategy(drainageStrategyId);
        _context.DrainageStrategies!.Remove(drainageStrategy);
        DeleteCaseLinks(drainageStrategyId);
        _context.SaveChanges();
        return _projectService.GetProjectDto(drainageStrategy.ProjectId);
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

    public ProjectDto UpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existing = GetDrainageStrategy(updatedDrainageStrategyDto.Id);
        var unit = _projectService.GetProject(existing.ProjectId).PhysicalUnit;

        DrainageStrategyAdapter.ConvertExisting(existing, updatedDrainageStrategyDto, unit, false);

        _context.DrainageStrategies!.Update(existing);
        _context.SaveChanges();
        return _projectService.GetProjectDto(existing.ProjectId);
    }

    public DrainageStrategyDto NewUpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto)
    {
        var existing = GetDrainageStrategy(updatedDrainageStrategyDto.Id);
        var unit = _projectService.GetProject(existing.ProjectId).PhysicalUnit;

        DrainageStrategyAdapter.ConvertExisting(existing, updatedDrainageStrategyDto, unit, false);

        var updatedDrainageStrategy = _context.DrainageStrategies!.Update(existing);
        _context.SaveChanges();
        return DrainageStrategyDtoAdapter.Convert(updatedDrainageStrategy.Entity, unit);
    }

    public DrainageStrategy GetDrainageStrategy(Guid drainageStrategyId)
    {
        var drainageStrategy = _context.DrainageStrategies!
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
            .FirstOrDefault(o => o.Id == drainageStrategyId);
        if (drainageStrategy == null)
        {
            throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
        }
        return drainageStrategy;
    }
}
