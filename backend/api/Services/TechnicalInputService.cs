using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class TechnicalInputService
{
    private readonly DcdDbContext _context;
    private readonly ProjectService _projectService;
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly WellProjectService _wellProjectService;
    private readonly ExplorationService _explorationService;
    private readonly SurfService _surfService;
    private readonly SubstructureService _substructureService;
    private readonly TransportService _transportService;
    private readonly TopsideService _topsideService;
    private readonly ExplorationWellService _explorationWellService;
    private readonly WellProjectWellService _wellProjectWellService;
    private readonly CostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<CaseService> _logger;

    public TechnicalInputService(DcdDbContext context, ProjectService projectService, CaseService caseService,
    DrainageStrategyService drainageStrategyService, WellProjectService wellProjectService, ExplorationService explorationService,
    SurfService surfService, SubstructureService substructureService, TransportService transportService, TopsideService topsideService,
    ExplorationWellService explorationWellService, WellProjectWellService wellProjectWellService,
    CostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILoggerFactory loggerFactory)
    {
        _context = context;

        _projectService = projectService;
        _caseService = caseService;

        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _explorationService = explorationService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _topsideService = topsideService;

        _explorationWellService = explorationWellService;
        _wellProjectWellService = wellProjectWellService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto UpdateTehnicalInput(TechnicalInputDto technicalInputDto)
    {
        // Update project
        // update operational well costs
        // update wells -> update exploration and wellproject costs
        var project = _projectService.GetProjectWithoutAssets(technicalInputDto.ProjectDto.ProjectId);
        var updatedProjectDto = UpdateProject(technicalInputDto.ProjectDto);

        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedCaseDto.ProjectId);
    }

    public ProjectDto UpdateProject(ProjectDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _projectService.GetProjectWithoutAssets(updatedDto.ProjectId);
        ProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Projects!.Update(item);
        return ProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public CaseDto UpdateExplorationOperationalWellCosts(ExplorationOperationalWellCostsDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _caseService.GetCase(updatedDto.Id);
        CaseAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Cases!.Update(item);
        return CaseDtoAdapter.Convert(updatedItem.Entity);
    }
    public CaseDto UpdateCase(CaseDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _caseService.GetCase(updatedDto.Id);
        CaseAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Cases!.Update(item);
        return CaseDtoAdapter.Convert(updatedItem.Entity);
    }

    public DrainageStrategyDto UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _drainageStrategyService.GetDrainageStrategy(updatedDto.Id);
        DrainageStrategyAdapter.ConvertExisting(item, updatedDto, unit, false);
        var updatedItem = _context.DrainageStrategies!.Update(item);
        return DrainageStrategyDtoAdapter.Convert(updatedItem.Entity, unit);
    }

    public WellProjectDto UpdateWellProject(WellProjectDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _wellProjectService.GetWellProject(updatedDto.Id);
        WellProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.WellProjects!.Update(item);
        return WellProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public ExplorationDto UpdateExploration(ExplorationDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _explorationService.GetExploration(updatedDto.Id);
        ExplorationAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Explorations!.Update(item);
        return ExplorationDtoAdapter.Convert(updatedItem.Entity);
    }

    public SurfDto UpdateSurf(SurfDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _surfService.GetSurf(updatedDto.Id);
        SurfAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Surfs!.Update(item);
        return SurfDtoAdapter.Convert(updatedItem.Entity);
    }

    public SubstructureDto UpdateSubstructure(SubstructureDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _substructureService.GetSubstructure(updatedDto.Id);
        SubstructureAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Substructures!.Update(item);
        return SubstructureDtoAdapter.Convert(updatedItem.Entity);
    }

    public TransportDto UpdateTransport(TransportDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _transportService.GetTransport(updatedDto.Id);
        TransportAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Transports!.Update(item);
        return TransportDtoAdapter.Convert(updatedItem.Entity);
    }
    public TopsideDto UpdateTopside(TopsideDto updatedDto)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }
        var item = _topsideService.GetTopside(updatedDto.Id);
        TopsideAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Topsides!.Update(item);
        return TopsideDtoAdapter.Convert(updatedItem.Entity);
    }
}
