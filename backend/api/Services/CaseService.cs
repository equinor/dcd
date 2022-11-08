using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService
{
    private readonly DcdDbContext _context;
    private readonly ILogger<CaseService> _logger;
    private readonly ProjectService _projectService;
    private readonly IServiceProvider _serviceProvider;

    public CaseService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory,
        IServiceProvider serviceProvider)
    {
        _context = context;
        _projectService = projectService;
        _serviceProvider = serviceProvider;
        _logger = loggerFactory.CreateLogger<CaseService>();
    }

    public ProjectDto CreateCase(CaseDto caseDto)
    {
        var case_ = CaseAdapter.Convert(caseDto);
        if (case_.DG4Date == DateTimeOffset.MinValue)
        {
            case_.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }

        var project = _projectService.GetProject(case_.ProjectId);
        case_.Project = project;
        _context.Cases!.Add(case_);
        _context.SaveChanges();
        return _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> NewCreateCase(CaseDto caseDto)
    {
        var drainageStrategyService = _serviceProvider.GetRequiredService<DrainageStrategyService>();
        var topsideService = _serviceProvider.GetRequiredService<TopsideService>();
        var surfService = _serviceProvider.GetRequiredService<SurfService>();
        var substructureService = _serviceProvider.GetRequiredService<SubstructureService>();
        var transportService = _serviceProvider.GetRequiredService<TransportService>();
        var explorationService = _serviceProvider.GetRequiredService<ExplorationService>();
        var wellProjectService = _serviceProvider.GetRequiredService<WellProjectService>();

        var caseItem = CaseAdapter.Convert(caseDto);
        var project = _projectService.GetProject(caseItem.ProjectId);
        caseItem.Project = project;
        caseItem.CapexFactorFeasibilityStudies = 0.015;
        caseItem.CapexFactorFEEDStudies = 0.015;

        var createdCase = _context.Cases!.Add(caseItem);
        await _context.SaveChangesAsync();

        var drainageStrategyDto = new DrainageStrategyDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Drainage strategy",
            Description = "",
        };
        var drainageStrategy = await
            drainageStrategyService.NewCreateDrainageStrategy(drainageStrategyDto, createdCase.Entity.Id);
        caseItem.DrainageStrategyLink = drainageStrategy.Id;

        var topsideDto = new TopsideDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Topside",
        };
        var topside = await topsideService.NewCreateTopside(topsideDto, createdCase.Entity.Id);
        caseItem.TopsideLink = topside.Id;

        var surfDto = new SurfDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Surf",
        };
        var surf = await surfService.NewCreateSurf(surfDto, createdCase.Entity.Id);
        caseItem.SurfLink = surf.Id;

        var substructureDto = new SubstructureDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Substructure",
        };
        var substructure = await substructureService.NewCreateSubstructure(substructureDto, createdCase.Entity.Id);
        caseItem.SubstructureLink = substructure.Id;

        var transportDto = new TransportDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Transport",
        };
        var transport = await transportService.NewCreateTransport(transportDto, createdCase.Entity.Id);
        caseItem.TransportLink = transport.Id;

        var explorationDto = new ExplorationDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "Exploration",
        };
        var exploration = await explorationService.NewCreateExploration(explorationDto, createdCase.Entity.Id);
        caseItem.ExplorationLink = exploration.Id;

        var wellProjectDto = new WellProjectDto
        {
            ProjectId = createdCase.Entity.ProjectId,
            Name = "WellProject",
        };
        var wellProject = await wellProjectService.NewCreateWellProject(wellProjectDto, createdCase.Entity.Id);
        caseItem.WellProjectLink = wellProject.Id;

        return _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> DuplicateCase(Guid caseId)
    {
        var caseItem = await GetCase(caseId);
        caseItem.Id = new Guid();
        if (caseItem.DG4Date == DateTimeOffset.MinValue)
        {
            caseItem.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero);
        }

        var project = _projectService.GetProject(caseItem.ProjectId);
        caseItem.Project = project;

        caseItem.Name = caseItem.Name + " - copy";
        _context.Cases!.Add(caseItem);
        await _context.SaveChangesAsync();
        return _projectService.GetProjectDto(project.Id);
    }

    public ProjectDto UpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id).GetAwaiter().GetResult();
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public CaseDto NewUpdateCase(CaseDto updatedCaseDto)
    {
        var caseItem = GetCase(updatedCaseDto.Id).GetAwaiter().GetResult();
        CaseAdapter.ConvertExisting(caseItem, updatedCaseDto);
        _context.Cases!.Update(caseItem);
        _context.SaveChanges();
        return CaseDtoAdapter.Convert(GetCase(caseItem.Id).Result);
    }

    public async Task<ProjectDto> DeleteCase(Guid caseId)
    {
        var caseItem = await GetCase(caseId);
        _context.Cases!.Remove(caseItem);
        await _context.SaveChangesAsync();
        return _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await _context.Cases!
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException($"Case {caseId} not found.");
        }

        return caseItem;
    }
}
