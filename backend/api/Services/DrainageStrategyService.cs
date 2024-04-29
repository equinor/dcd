using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;


namespace api.Services;

public class DrainageStrategyService : IDrainageStrategyService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<DrainageStrategyService> _logger;
    private readonly IMapper _mapper;

    public DrainageStrategyService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper)
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<DrainageStrategyService>();
        _mapper = mapper;
    }

    public async Task<ProjectDto> CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
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

    public async Task<DrainageStrategyDto> CopyDrainageStrategy(Guid drainageStrategyId, Guid sourceCaseId)
    {
        var source = await GetDrainageStrategy(drainageStrategyId);
        var unit = (await _projectService.GetProject(source.ProjectId)).PhysicalUnit;

        var newDrainageStrategyDto = _mapper.Map<DrainageStrategy, DrainageStrategyDto>(
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

        // var drainageStrategy = await NewCreateDrainageStrategy(newDrainageStrategyDto, sourceCaseId);
        // var dto = DrainageStrategyDtoAdapter.Convert(drainageStrategy, unit);

        // return dto;
        return new DrainageStrategyDto();
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
            .FirstOrDefaultAsync(o => o.Id == drainageStrategyId);
        if (drainageStrategy == null)
        {
            throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
        }
        return drainageStrategy;
    }
}
