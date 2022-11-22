using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

namespace api.Services;

public class CaseWithAssetsService
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
    private readonly ILogger<CaseService> _logger;

    public CaseWithAssetsService(DcdDbContext context, ProjectService projectService, CaseService caseService,
    DrainageStrategyService drainageStrategyService, WellProjectService wellProjectService, ExplorationService explorationService,
    SurfService surfService, SubstructureService substructureService, TransportService transportService, TopsideService topsideService,
     ExplorationWellService explorationWellService, WellProjectWellService wellProjectWellService,
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

        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    // Add wellprojectwell and explorationwell 
    public ProjectDto UpdateCaseWithAssets(CaseWithAssetsWrapperDto wrapper)
    {
        var project = _projectService.GetProject(wrapper.CaseDto.ProjectId);

        var updatedCaseDto = UpdateCase(wrapper.CaseDto);

        var updatedDrainageStrategyDto = UpdateDrainageStrategy(wrapper.DrainageStrategyDto, project.PhysicalUnit);
        var updatedWellProjectDto = UpdateWellProject(wrapper.WellProjectDto);
        var updatedExplorationDto = UpdateExploration(wrapper.ExplorationDto);
        var updatedSurfDto = UpdateSurf(wrapper.SurfDto);
        var updatedSubstructureDto = UpdateSubstructure(wrapper.SubstructureDto);
        var updatedTransportDto = UpdateTransport(wrapper.TransportDto);
        var updatedTopsideDto = UpdateTopside(wrapper.TopsideDto);

        if (wrapper.ExplorationWellDto?.Length > 0)
        {
            // Create / update explorationWell
        }

        if (wrapper.WellProjectWellDtos?.Length > 0)
        {
            // Create / update WellProjectWell
        }

        _context.SaveChanges();
        return _projectService.GetProjectDto(updatedCaseDto.ProjectId);
    }

    public ExplorationWellDto[]? CreateMultipleExplorationWells(ExplorationWellDto[] explorationWellDtos)
    {
        var explorationId = explorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            projectDto = CreateExplorationWell(explorationWellDto);
        }
        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    public ProjectDto CreateExplorationWell(ExplorationWellDto explorationWellDto)
    {
        var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
        _context.ExplorationWell!.Add(explorationWell);
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == explorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public ProjectDto UpdateExplorationWell(ExplorationWellDto updatedExplorationWellDto)
    {
        var existingExplorationWell = _explorationWellService.GetExplorationWell(updatedExplorationWellDto.WellId, updatedExplorationWellDto.ExplorationId);
        ExplorationWellAdapter.ConvertExisting(existingExplorationWell, updatedExplorationWellDto);
        if (updatedExplorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
        {
            _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
        }

        _context.ExplorationWell!.Update(existingExplorationWell);
        var projectId = _context.Explorations!.FirstOrDefault(c => c.Id == updatedExplorationWellDto.ExplorationId)?.ProjectId;
        if (projectId != null)
        {
            return _projectService.GetProjectDto((Guid)projectId);
        }
        throw new NotFoundInDBException();
    }

    public ExplorationWellDto[]? UpdateMultpleExplorationWells(ExplorationWellDto[] updatedExplorationWellDtos, Guid caseId)
    {
        var explorationId = updatedExplorationWellDtos.FirstOrDefault()?.ExplorationId;
        ProjectDto? projectDto = null;
        foreach (var explorationWellDto in updatedExplorationWellDtos)
        {
            projectDto = UpdateExplorationWell(explorationWellDto);
        }

        var costProfileHelper = _serviceProvider.GetRequiredService<CostProfileFromDrillingScheduleHelper>();
        var explorationDto = costProfileHelper.UpdateExplorationCostProfilesForCase(caseId);

        UpdateExploration(explorationDto);

        if (projectDto != null && explorationId != null)
        {
            return projectDto.Explorations?.FirstOrDefault(e => e.Id == explorationId)?.ExplorationWells?.ToArray();
        }
        return null;
    }

    // public ProjectDto CreateCaseWithAssets(CaseDto caseDto)
    // {
    //     var drainageStrategyService = _serviceProvider.GetRequiredService<DrainageStrategyService>();
    //     var topsideService = _serviceProvider.GetRequiredService<TopsideService>();
    //     var surfService = _serviceProvider.GetRequiredService<SurfService>();
    //     var substructureService = _serviceProvider.GetRequiredService<SubstructureService>();
    //     var transportService = _serviceProvider.GetRequiredService<TransportService>();
    //     var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
    //     var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();

    //     var case_ = CaseAdapter.Convert(caseDto);
    //     var project = _projectService.GetProject(case_.ProjectId);
    //     case_.Project = project;
    //     case_.CapexFactorFeasibilityStudies = 0.015;
    //     case_.CapexFactorFEEDStudies = 0.015;

    //     var createdCase = _context.Cases!.Add(case_);
    //     _context.SaveChanges();

    //     var drainageStrategyDto = new DrainageStrategyDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Drainage strategy",
    //         Description = ""
    //     };
    //     var drainageStrategy = drainageStrategyService.NewCreateDrainageStrategy(drainageStrategyDto, createdCase.Entity.Id);
    //     case_.DrainageStrategyLink = drainageStrategy.Id;

    //     var topsideDto = new TopsideDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Topside",
    //     };
    //     var topside = topsideService.NewCreateTopside(topsideDto, createdCase.Entity.Id);
    //     case_.TopsideLink = topside.Id;

    //     var surfDto = new SurfDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Surf",
    //     };
    //     var surf = surfService.NewCreateSurf(surfDto, createdCase.Entity.Id);
    //     case_.SurfLink = surf.Id;

    //     var substructureDto = new SubstructureDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Substructure",
    //     };
    //     var substructure = substructureService.NewCreateSubstructure(substructureDto, createdCase.Entity.Id);
    //     case_.SubstructureLink = substructure.Id;

    //     var transportDto = new TransportDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Transport",
    //     };
    //     var transport = transportService.NewCreateTransport(transportDto, createdCase.Entity.Id);
    //     case_.TransportLink = transport.Id;

    //     var explorationDto = new ExplorationDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "Exploration",
    //     };
    //     var exploration = explorationService.NewCreateExploration(explorationDto, createdCase.Entity.Id);
    //     case_.ExplorationLink = exploration.Id;

    //     var wellProjectDto = new WellProjectDto
    //     {
    //         ProjectId = createdCase.Entity.ProjectId,
    //         Name = "WellProject",
    //     };
    //     var wellProject = wellProjectService.NewCreateWellProject(wellProjectDto, createdCase.Entity.Id);
    //     case_.WellProjectLink = wellProject.Id;

    //     return _projectService.GetProjectDto(project.Id);
    // }

    // public ProjectDto DuplicateCase(Guid caseId)
    // {
    //     var drainageStrategyService = _serviceProvider.GetRequiredService<DrainageStrategyService>();

    //     var topsideService = _serviceProvider.GetRequiredService<TopsideService>();
    //     var surfService = _serviceProvider.GetRequiredService<SurfService>();
    //     var substructureService = _serviceProvider.GetRequiredService<SubstructureService>();
    //     var transportService = _serviceProvider.GetRequiredService<TransportService>();

    //     var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
    //     var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();

    //     var wellProjectWellService = _serviceProvider.GetRequiredService<WellProjectWellService>();
    //     var explorationWellService = _serviceProvider.GetRequiredService<ExplorationWellService>();

    //     var caseItem = GetCase(caseId);
    //     var sourceWellProjectId = caseItem.WellProjectLink;
    //     var sourceExplorationId = caseItem.ExplorationLink;
    //     caseItem.Id = new Guid();
    //     if (caseItem.DG4Date == DateTimeOffset.MinValue)
    //     {
    //         caseItem.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
    //     }
    //     var project = _projectService.GetProject(caseItem.ProjectId);
    //     caseItem.Project = project;

    //     caseItem.Name += " - copy";
    //     _context.Cases!.Add(caseItem);

    //     drainageStrategyService.CopyDrainageStrategy(caseItem.DrainageStrategyLink, caseItem.Id);
    //     topsideService.CopyTopside(caseItem.TopsideLink, caseItem.Id);
    //     surfService.CopySurf(caseItem.SurfLink, caseItem.Id);
    //     substructureService.CopySubstructure(caseItem.SubstructureLink, caseItem.Id);
    //     transportService.CopyTransport(caseItem.TransportLink, caseItem.Id);
    //     var newWellProject = wellProjectService.CopyWellProject(caseItem.WellProjectLink, caseItem.Id);
    //     var newExploration = explorationService.CopyExploration(caseItem.ExplorationLink, caseItem.Id);

    //     wellProjectWellService.CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
    //     explorationWellService.CopyExplorationWell(sourceExplorationId, newExploration.Id);

    //     _context.SaveChanges();
    //     return _projectService.GetProjectDto(project.Id);
    // }

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
